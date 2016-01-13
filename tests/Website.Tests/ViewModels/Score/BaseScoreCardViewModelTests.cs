namespace QOAM.Website.Tests.ViewModels.Score
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core;
    using Website.ViewModels.Score;
    using Xunit;

    public class BaseScoreCardViewModelTests
    {
        [Fact]
        public void NotValidWhenQuestionScoresIsNull()
        {
            // Arrange
            var model = new BaseScoreCardViewModel { QuestionScores = null };

            // Act
            var isValid = Validator.TryValidateObject(model, new ValidationContext(model), null);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void NotValidWhenQuestionScoresIsEmpty()
        {
            // Arrange
            var model = new BaseScoreCardViewModel { QuestionScores = new List<QuestionScoreViewModel>() };

            // Act
            var isValid = Validator.TryValidateObject(model, new ValidationContext(model), null);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void NotValidWhenAllQuestionScoresAreUndecided()
        {
            // Arrange
            var model = new BaseScoreCardViewModel
            {
                QuestionScores = new List<QuestionScoreViewModel>
                {
                    new QuestionScoreViewModel { Score = Score.Undecided },
                    new QuestionScoreViewModel { Score = Score.Undecided },
                    new QuestionScoreViewModel { Score = Score.Undecided },
                }
            };

            // Act
            var isValid = Validator.TryValidateObject(model, new ValidationContext(model), null);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void NotValidWhenOneQuestionScoreIsUndecided()
        {
            // Arrange
            var model = new BaseScoreCardViewModel
            {
                QuestionScores = new List<QuestionScoreViewModel>
                {
                    new QuestionScoreViewModel { Score = Score.Absent },
                    new QuestionScoreViewModel { Score = Score.Excellent },
                    new QuestionScoreViewModel { Score = Score.Undecided },
                }
            };

            // Act
            var isValid = Validator.TryValidateObject(model, new ValidationContext(model), null);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ValidWhenAllQuestionScoresAreNotUndecided()
        {
            // Arrange
            var model = new BaseScoreCardViewModel
            {
                QuestionScores = new List<QuestionScoreViewModel>
                {
                    new QuestionScoreViewModel { Score = Score.Absent },
                    new QuestionScoreViewModel { Score = Score.Excellent },
                    new QuestionScoreViewModel { Score = Score.Good },
                }
            };

            // Act
            var isValid = Validator.TryValidateObject(model, new ValidationContext(model), null);

            // Assert
            Assert.True(isValid);
        }
    }
}