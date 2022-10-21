using System.Collections.Generic;
using LHKorolevTgB.Model.Test;
using LHKorolevTgB.Model;

namespace LHKorolevTgB.Controller.Builder
{
    internal static class TestControllerBuilder
    {
        public static TestController BuildFromXml(TestControllerInXml testxml)
        {
            return new TestController(BDT(testxml.dataTest), testxml.originalCountOfQuestion, testxml.typesOfKeysRequest, testxml.responseWithCorrectAnswer, testxml.responseWithUncorrectAnswer, testxml.responseOfScore);
        }

        private static TestQuestion BTQ(TestQuestionInXml tq)
        {
            return new TestQuestion(tq.Question, tq.uncorrectAnswers, tq.correctAnswer);
        }

        private static Context BSPQ(ContextInXml spqixml)
        {
            List<TestQuestion> ltq = new List<TestQuestion>();
            foreach (TestQuestionInXml el in spqixml.questions)
            {
                ltq.Add(BTQ(el));
            }
            return new Context(spqixml.Keys, ltq, spqixml.description);
        }
        private static DataTest BDT(DataTestInXml dtxml)
        {
            List<Context> lspq = new List<Context>();
            foreach (ContextInXml el in dtxml.Questions)
            {
                lspq.Add(BSPQ(el));
            }
            return new DataTest(lspq, dtxml.MasterKeys, dtxml.Keys);

        }
    }
}
