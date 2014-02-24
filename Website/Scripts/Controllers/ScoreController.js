var ScoreController = (function () {
    function ScoreController() {
    }
    ScoreController.prototype.index = function (journalTitlesUrl, journalIssnsUrl, journalPublishersUrl) {
        $('#Title').typeahead({ remote: journalTitlesUrl + '?query=%QUERY' });
        $('#Issn').typeahead({ remote: journalIssnsUrl + '?query=%QUERY' });
        $('#Publisher').typeahead({ remote: journalPublishersUrl + '?query=%QUERY' });
    };

    ScoreController.prototype.details = function () {

        $('.scoreCardAspects a[data-select-tab="true"]').on('click', function () {
            $('#scoreTabs a[href="' + $(this).attr('href') + '"]').tab('show');
            return false;
        });
    };

    ScoreController.prototype.journal = function (data, saveScoreCardUrl, profileUrl) {

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
            var _isPublishedWithinYear = data ? data.Submitted : false;
            self.IsPublishedWithinYear = ko.observable(_isPublishedWithinYear);
            self.IsOutDatedPublication = ko.observable(data.Price.Amount != null && data ? !_isPublishedWithinYear : false); // we can't reuse the self.IsPublishedWithinYear for !IsPublishedWithinYear(), since it will set the corresponding question on visible on initialization            
            self.IsOutDatedPublicationArticle = ko.observable(data ? self.IsOutDatedPublicationArticle : false);
            self.IsOutDatedPublicationPage = ko.observable(data ? self.IsOutDatedPublicationPage : false);
            self.IsPublishedWithinYearScored = ko.observable(false);
            self.IsPublishedWithinYearRadio = ko.computed({
                read: function (v) {
                    return data.Submitted;
                },
                write: function (v) {
                    if (v == "true") {
                        self.Submitted(true);
                        self.IsPublishedWithinYear(true);
                        self.IsOutDatedPublication(false);
                    } else {
                        self.Submitted(false);
                        self.IsPublishedWithinYear(false);
                        self.IsOutDatedPublication(true);
                    }
                }
            });

            ko.mapping.fromJS(scoreCardData, {}, self);

            var isEditorComment = $('#isEditorComment').val();

            var categories = ko.utils.arrayGetDistinctValues(ko.utils.arrayMap(self.QuestionScores(), function (item) {
                return item.QuestionCategory();
            }));

            self.publishing = ko.observable(false);

            self.getRequestVerificationToken = function () {
                return $('#journalScoreForm input[name="__RequestVerificationToken"]').val();
            },

            self.storeScoreCard = function () {
                if (currentXhr != null) {
                    currentXhr.abort();
                }

                currentXhr = $.ajax({
                    type: "POST",
                    url: saveScoreCardUrl,
                    contentType: 'application/json',
                    data: ko.utils.stringifyJson(ko.mapping.toJS(self)),
                    dataType: 'json',
                    headers: { '__RequestVerificationToken': self.getRequestVerificationToken() }
                })
                .done(function () {
                    $('#publishedModal')
                        .modal()
                        .on('hide.bs.modal', function () {
                            window.location.href = profileUrl;
                        });
                })
                .fail(function () {
                    alert('Something failed!');
                    self.publishing(false);
                });
            };

            self.publishScoreCard = function () {
                self.publishing(true);
                self.storeScoreCard();
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
                return self.progress() >= 100 && !self.publishing() && self.IsPublishedWithinYearScored;
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
                self.IsPublishedWithinYear(v);
                self.IsOutDatedPublication(!v);
                self.IsPublishedWithinYearScored(v);
            });

            $("input[type=radio][name=outdatedPublication]").bind("click", function () {
                var v = $(this).val();
                self.Submitted(false);
                self.Price.FeeType = v;
                self.IsPublishedWithinYearScored(true);
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
                console.log(data);
                $("input[type=radio][name=IsPublishedWithinYearRadio]").each(function (index) {
                    if ($(this).val() == data.Submitted.toString()) {
                        $(this).prop('checked', true);
                    }
                });

                $("input[type=radio][name=outdatedPublication]").each(function (index) {
                    if (data.Price.FeeType == index) {
                        $(this).prop('checked', true);
                        $(this).trigger("click");
                    }
                });
            }            
        };

        var viewModel = new scoreCardModel(data);
        ko.applyBindings(viewModel);
    };
    return ScoreController;
})();