var ScoreController = (function () {
    function ScoreController() {
    }
    ScoreController.prototype.index = function (journalTitlesUrl, journalIssnsUrl, journalPublishersUrl, subjectsUrl, languagesUrl) {
        createTypeahead('#Title', journalTitlesUrl);
        createTypeahead('#Issn', journalIssnsUrl);
        createTypeahead('#Publisher', journalPublishersUrl);
        //createTypeahead('input.search-discipline', subjectsUrl);
        createTypeahead('input.search-language', languagesUrl);

        $("#SelectedDisciplines").chosen({
            search_contains: true,
            placeholder_text_multiple: "Search by discipline"
        });

        $('.remove-discipline').on('click', function (e) {
            $(this).closest('li').remove();

            $('#disciplines input').each(function (index, element) {
                $(element).attr('name', 'Disciplines[' + index + ']');
            });

            e.preventDefault();
            $('#search-form').submit();

            return false;
        });

        $('.remove-language').on('click', function (e) {
            $(this).closest('li').remove();

            $('#languages input').each(function (index, element) {
                $(element).attr('name', 'Languages[' + index + ']');
            });

            e.preventDefault();
            $('#search-form').submit();

            return false;
        });
    };

    ScoreController.prototype.baseScoreCardDetails = function () {

        $('.scoreCardAspects a[data-select-tab="true"]').on('click', function () {
            $('#scoreTabs a[href="' + $(this).attr('href') + '"]').tab('show');
            return false;
        });
    };

    ScoreController.prototype.valuationScoreCardDetails = function () {

        $('.scoreCardAspects a[data-select-tab="true"]').on('click', function () {
            $('#scoreTabs a[href="' + $(this).attr('href') + '"]').tab('show');
            return false;
        });
    };

    ScoreController.prototype.baseScoreCard = function (data, saveScoreCardUrl, profileUrl) {
        var setupSpinner = function() {
            var opts = {
                lines: 11,
                length: 4,
                width: 3,
                radius: 6,
                corners: 1,
                rotate: 0,
                direction: 1,
                color: '#000',
                speed: 1,
                trail: 60,
                shadow: false,
                hwaccel: false,
                className: 'spinner',
                zIndex: 2e9,
                top: 'auto',
                left: 'auto'
            };
            var target = document.getElementById('spinner');
            var spinner = new Spinner(opts).spin(target);
        };

        setupSpinner();

        var currentXhr = null;
        var valuationScoreCategory = 4;

        $('.scoreCardAspects a[data-select-tab="true"], #OverviewTab a[data-select-tab="true"]').on('click', function () {
            $('#scoreTabs a[href="' + $(this).attr('href') + '"]').tab('show');
            return false;
        });

        var categoryScoreModel = function (questionCategory, questionScores) {
            var self = this;

            var questions = ko.utils.arrayFilter(questionScores, function (item) {
                return item.QuestionCategory() == questionCategory;
            });

            self.completed = ko.computed(function () {
                return ko.utils.arrayFirst(questions, function (item) {
                    return item.Score() == 0;
                }) == null;
            });

            self.partOfBaseScore = function () {
                return questionCategory != valuationScoreCategory;
            },

            self.score = ko.computed(function () {
                if (questions.length == 0) {
                    return 0;
                }

                var totalScore = 0;

                ko.utils.arrayForEach(questions, function (item) {
                    totalScore += item.Score();
                });

                return Math.round(totalScore / questions.length * 10) / 10;
            });
        };

        var questionScoreModel = function (questionKey, questionScores) {
            var self = this;
            var question = ko.utils.arrayFirst(questionScores, function (item) {
                return item.QuestionKey() == questionKey;
            });

            self.score = ko.computed({
                read: function () {
                    return question.Score();
                },
                write: function (value) {
                    question.Score(+value);
                }
            });
        };

        var scoreCardModel = function (scoreCardData) {

            var self = this;
            self.IsOutDatedPublicationArticle = ko.observable(data ? self.IsOutDatedPublicationArticle : false);
            self.IsOutDatedPublicationPage = ko.observable(data ? self.IsOutDatedPublicationPage : false);
           
            ko.mapping.fromJS(scoreCardData, {}, self);
            
            var categories = ko.utils.arrayGetDistinctValues(ko.utils.arrayMap(self.QuestionScores(), function (item) {
                return item.QuestionCategory();
            }));

            self.publishing = ko.observable(false);
            self.storingScoreCard = ko.observable(false);
            
            self.getRequestVerificationToken = function () {
                return $('#journalScoreForm input[name="__RequestVerificationToken"]').val();
            },

            self.storeScoreCard = function () {
                if (currentXhr != null) {
                    currentXhr.abort();
                }

                self.storingScoreCard(true);
                
                currentXhr = $.ajax({
                    type: "POST",
                    url: saveScoreCardUrl,
                    contentType: 'application/json',
                    data: ko.utils.stringifyJson(ko.mapping.toJS(self)),
                    dataType: 'json',
                    headers: { '__RequestVerificationToken': self.getRequestVerificationToken() }
                })
                .always(function() {
                    self.storingScoreCard(false);
                });

                return currentXhr;
            };

            self.publishScoreCard = function () {
                // We set this value to indicating that we are in the processing of publishing a score card
                self.publishing(true);

                // We set this value to let the backend know that this is a publish action, not only a mere
                // updating of the 
                self.Publish(true);

                self.storeScoreCard()
                    .done(function () {
                        $('#publishedModal')
                            .modal()
                            .on('hide.bs.modal', function () {
                                window.location.href = profileUrl;
                            });
                    })
                    .fail(function () {
                        alert('An error occured.\nPlease ensure you scored all questions and try again.');
                        self.publishing(false);
                    });
            };

            self.categoryScores = ko.observableArray();
            self.questionScores = ko.observableArray();
            
            self.baseScore = ko.computed(function () {
                return ko.utils.arrayMap(ko.utils.arrayFilter(self.categoryScores(), function (item) {
                    return item.partOfBaseScore() && item.completed();
                }),
                    function (item) {
                        return item.score();
                    }
                ).sort()[0] || 0;
            });

            self.priceQuesionAnswered = ko.computed(function() {
                return typeof self.Price.FeeType == 'function' && self.Price.FeeType() == null ? 0 : 1;
            });

            self.progress = ko.computed(function () {
                var numberOfScoredQuestions = ko.utils.arrayFilter(self.QuestionScores(), function (item) {
                    return item.Score() > 0;
                }).length;
                
                // Note: the added +1 is because the non-score price question also needs to be taken into account
                var totalNumberOfQuestions = self.QuestionScores().length + 1;
                var priceQuestionAnswered = self.priceQuesionAnswered();
                
                return Math.round(((numberOfScoredQuestions + self.priceQuesionAnswered()) / totalNumberOfQuestions) * 100.0);
            });

            self.progressPercentage = ko.computed(function () {
                return self.progress() + '%';
            });

            self.progressClass = ko.computed(function () {
                var progress = self.progress();

                if (progress < 50) {
                    return 'progress-bar-danger';
                }
                else if (progress < 100) {
                    return 'progress-bar-warning';
                }

                return 'progress-bar-success';
            });

            ko.utils.arrayForEach(categories, function (category) {
                self.categoryScores.push(new categoryScoreModel(category, self.QuestionScores()));
            });

            ko.utils.arrayForEach(self.QuestionScores(), function (questionScore) {
                self.questionScores.push(new questionScoreModel(questionScore.QuestionKey(), self.QuestionScores()));
            });

            self.canPublish = ko.computed(function () {
                return self.progress() >= 100 && !self.publishing();
            });
            
            $("input[type=radio][name=outdatedPublication]").bind("click", function () {
                var v = $(this).val();
                self.Price.FeeType = v;
                switch (v) {
                    case "Article":
                        self.IsOutDatedPublicationArticle(true);
                        self.IsOutDatedPublicationPage(false);
                        break;
                    case "Page":
                        self.IsOutDatedPublicationArticle(false);
                        self.IsOutDatedPublicationPage(true);
                        break;
                    default:
                        self.IsOutDatedPublicationArticle(false);
                        self.IsOutDatedPublicationPage(false);
                        break;
                }
            });

            if (data.Price.Amount != null || self.progress() >= 100) {
                $("input[type=radio][name=outdatedPublication]").each(function (index) {
                    if (data.Price.FeeType == index) {
                        $(this).prop('checked', true);
                        $(this).trigger("click");
                    }
                });
            }
            
            // We only do auto-saving for unpublished (state equal to zero) score cards.
            if (data.State == 0) {
                var savedElement = $('#saved');

                self.questionScoresDirtyFlag = new ko.dirtyFlag(self.questionScores, false);
                self.remarksDirtyFlag = new ko.dirtyFlag(self.Remarks, false);
                self.editorDirtyFlag = new ko.dirtyFlag(self.Editor, false);
                self.priceDirtyFlag = new ko.dirtyFlag(self.Price, false);
            
                self.isDirty = ko.computed(function () {
                    return self.questionScoresDirtyFlag.isDirty() ||
                           self.remarksDirtyFlag.isDirty() ||
                           self.editorDirtyFlag.isDirty() || 
                           self.priceDirtyFlag.isDirty();
                }, self).extend({ throttle: 2000 });
            
                self.isDirty.subscribe(function (newIsDirtyValue) {
                    if (newIsDirtyValue && !self.publishing()) {

                        // Ensure that the score card will not be published
                        self.Publish(false);

                        savedElement.hide();
                        
                        self.storeScoreCard()
                                .done(function () {
                                    savedElement.show().delay(200).fadeOut(2000);

                                    // Reset the dirty flag to the just updated question scores
                                    self.questionScoresDirtyFlag.reset(self.questionScores);
                                    self.remarksDirtyFlag.reset(self.Remarks);
                                    self.editorDirtyFlag.reset(self.Editor);
                                    self.priceDirtyFlag.reset(self.Price);
                                });
                    }
                });
            }
        };

        var viewModel = new scoreCardModel(data);
        ko.applyBindings(viewModel);
    };


    ScoreController.prototype.valuationScoreCard = function (data, saveScoreCardUrl, profileUrl) {
        var setupSpinner = function () {
            var opts = {
                lines: 11,
                length: 4,
                width: 3,
                radius: 6,
                corners: 1,
                rotate: 0,
                direction: 1,
                color: '#000',
                speed: 1,
                trail: 60,
                shadow: false,
                hwaccel: false,
                className: 'spinner',
                zIndex: 2e9,
                top: 'auto',
                left: 'auto'
            };
            var target = document.getElementById('spinner');
            var spinner = new Spinner(opts).spin(target);
        };

        setupSpinner();

        var currentXhr = null;
        var valuationScoreCategory = 4;

        $('.scoreCardAspects a[data-select-tab="true"], #OverviewTab a[data-select-tab="true"]').on('click', function () {
            $('#scoreTabs a[href="' + $(this).attr('href') + '"]').tab('show');
            return false;
        });

        var categoryScoreModel = function (questionCategory, questionScores) {
            var self = this;

            var questions = ko.utils.arrayFilter(questionScores, function (item) {
                return item.QuestionCategory() == questionCategory;
            });

            self.completed = ko.computed(function () {
                return ko.utils.arrayFirst(questions, function (item) {
                    return item.Score() == 0;
                }) == null;
            });

            self.partOfBaseScore = function () {
                return questionCategory != valuationScoreCategory;
            },

            self.score = ko.computed(function () {
                if (questions.length == 0) {
                    return 0;
                }

                var totalScore = 0;

                ko.utils.arrayForEach(questions, function (item) {
                    totalScore += item.Score();
                });

                return Math.round(totalScore / questions.length * 10) / 10;
            });
        };

        var questionScoreModel = function (questionKey, questionScores) {
            var self = this;
            var question = ko.utils.arrayFirst(questionScores, function (item) {
                return item.QuestionKey() == questionKey;
            });

            self.score = ko.computed({
                read: function () {
                    return question.Score();
                },
                write: function (value) {
                    question.Score(+value);
                }
            });
        };

        var scoreCardModel = function (scoreCardData) {

            var self = this;
            
            ko.mapping.fromJS(scoreCardData, {}, self);

            var isEditorComment = $('#isEditorComment').val();

            var categories = ko.utils.arrayGetDistinctValues(ko.utils.arrayMap(self.QuestionScores(), function (item) {
                return item.QuestionCategory();
            }));

            self.publishing = ko.observable(false);
            self.storingScoreCard = ko.observable(false);

            self.getRequestVerificationToken = function () {
                return $('#journalScoreForm input[name="__RequestVerificationToken"]').val();
            },

            self.storeScoreCard = function () {
                if (currentXhr != null) {
                    currentXhr.abort();
                }

                self.storingScoreCard(true);

                currentXhr = $.ajax({
                    type: "POST",
                    url: saveScoreCardUrl,
                    contentType: 'application/json',
                    data: ko.utils.stringifyJson(ko.mapping.toJS(self)),
                    dataType: 'json',
                    headers: { '__RequestVerificationToken': self.getRequestVerificationToken() }
                })
                .always(function () {
                    self.storingScoreCard(false);
                });

                return currentXhr;
            };

            self.publishScoreCard = function () {
                // We set this value to indicating that we are in the processing of publishing a score card
                self.publishing(true);

                // We set this value to let the backend know that this is a publish action, not only a mere
                // updating of the 
                self.Publish(true);

                self.storeScoreCard()
                    .done(function () {
                        $('#publishedModal')
                            .modal()
                            .on('hide.bs.modal', function () {
                                window.location.href = profileUrl;
                            });
                    })
                    .fail(function () {
                        alert('An error occured.\nPlease ensure you scored all questions and try again.');
                        self.publishing(false);
                    });
            };

            self.categoryScores = ko.observableArray();
            self.questionScores = ko.observableArray();

            self.valuationScore = ko.computed(function () {
                return ko.utils.arrayMap(ko.utils.arrayFilter(self.categoryScores(), function (item) {
                    return !item.partOfBaseScore() && item.completed();
                }),
                    function (item) {
                        return item.score();
                    }
                ).sort()[0] || 0;
            });

            self.progress = ko.computed(function () {
                var numberOfScoredQuestions = ko.utils.arrayFilter(self.QuestionScores(), function (item) {
                    return item.Score() > 0;
                }).length;

                var totalNumberOfQuestions = self.QuestionScores().length;
                return Math.round((numberOfScoredQuestions / totalNumberOfQuestions) * 100.0);
            });

            self.progressPercentage = ko.computed(function () {
                return self.progress() + '%';
            });

            self.progressClass = ko.computed(function () {
                var progress = self.progress();

                if (progress < 50) {
                    return 'progress-bar-danger';
                }
                else if (progress < 100) {
                    return 'progress-bar-warning';
                }

                return 'progress-bar-success';
            });

            ko.utils.arrayForEach(categories, function (category) {
                self.categoryScores.push(new categoryScoreModel(category, self.QuestionScores()));
            });

            ko.utils.arrayForEach(self.QuestionScores(), function (questionScore) {
                self.questionScores.push(new questionScoreModel(questionScore.QuestionKey(), self.QuestionScores()));
            });

            self.canPublish = ko.computed(function () {
                return self.progress() >= 100 && !self.publishing();
            });

            self.onRemarksFocus = function () {
                if ($(this).val() == isEditorComment) {
                    $(this).val('');
                }
            };

            self.onRemarksBlur = function () {
                if ($(this).val() == '') {
                    $(this).val(isEditorComment);
                }
            };

            self.Editor.subscribe(function (checked) {
                var remarksElement = $('#remarks');

                if (checked) {
                    remarksElement.val(isEditorComment);
                    remarksElement.on('focus', self.onRemarksFocus)
								  .on('blur', self.onRemarksBlur);
                } else {
                    remarksElement.val('');
                    remarksElement.off('focus', self.onRemarksFocus)
								  .off('blur', self.onRemarksBlur);
                }
            });

            // replacing this for the knockout version. For some reason i get checked event problems 
            $("input[type=radio][name=IsPublishedWithinYearRadio]").bind("click", function () {
                var v = $(this).val() == "true";

                self.Submitted(v);
            });

            if (data.Price.Amount != null || self.progress() >= 100) {
                $("input[type=radio][name=IsPublishedWithinYearRadio]").each(function (index) {
                    if ($(this).val() == data.Submitted.toString()) {
                        $(this).prop('checked', true);
                    }
                });
            }

            // We only do auto-saving for unpublished (state equal to zero) score cards.
            if (data.State == 0) {
                var savedElement = $('#saved');

                self.questionScoresDirtyFlag = new ko.dirtyFlag(self.questionScores, false);
                self.remarksDirtyFlag = new ko.dirtyFlag(self.Remarks, false);
                self.editorDirtyFlag = new ko.dirtyFlag(self.Editor, false);
                self.submittedDirtyFlag = new ko.dirtyFlag(self.Submitted, false);
                self.priceDirtyFlag = new ko.dirtyFlag(self.Price, false);

                self.isDirty = ko.computed(function () {
                    return self.questionScoresDirtyFlag.isDirty() ||
                           self.remarksDirtyFlag.isDirty() ||
                           self.editorDirtyFlag.isDirty() ||
                           self.submittedDirtyFlag.isDirty() ||
                           self.priceDirtyFlag.isDirty();
                }, self).extend({ throttle: 2000 });

                self.isDirty.subscribe(function (newIsDirtyValue) {
                    if (newIsDirtyValue && !self.publishing()) {

                        // Ensure that the score card will not be published
                        self.Publish(false);

                        savedElement.hide();

                        self.storeScoreCard()
                                .done(function () {
                                    savedElement.show().delay(200).fadeOut(2000);

                                    // Reset the dirty flag to the just updated question scores
                                    self.questionScoresDirtyFlag.reset(self.questionScores);
                                    self.remarksDirtyFlag.reset(self.Remarks);
                                    self.editorDirtyFlag.reset(self.Editor);
                                    self.submittedDirtyFlag.reset(self.Submitted);
                                    self.priceDirtyFlag.reset(self.Price);
                                });
                    }
                });
            }
        };

        var viewModel = new scoreCardModel(data);
        ko.applyBindings(viewModel);
    };

    return ScoreController;
})();