using System.Collections.Generic;

namespace LHKorolevTgB.Controller
{
    class MainController
    {
        public string TextResponse { get; private set; }
        public string MediaResponse { get; private set; }
        public List<List<string>> NextPossibleTextRequests { get; private set; }

        private TestController tc;
        private EncyclopediaController ec;
        private readonly string keyWordForReturn;
        private readonly string textInMenu;
        private readonly string mediaInMenu;
        private readonly string keyWordToSTest;
        private readonly string keyWordToSEncyclopedya;

        private bool isFirstRequest = true;

        private List<List<string>> AddRC(List<List<string>> ll)
        {
            ll.Add(new List<string> { keyWordForReturn });
            return ll;
        }

        private void PrepareMenu()
        {
            TextResponse = textInMenu;
            MediaResponse = mediaInMenu;
            NextPossibleTextRequests = new List<List<string>> { new List<string> { keyWordToSEncyclopedya }, new List<string> { keyWordToSTest } };
            tc.ResetController();
            ec.ResetController();

        }
        private void PrepareEnc(string request)
        {
            ec.ProcessingRespose(request);
            TextResponse = ec.Request;
            MediaResponse = ec.MediaRequest;
            NextPossibleTextRequests = AddRC(ec.NextResponses);
        }

        private void PrepareTest(string request)
        {
            tc.ProcessingRespose(request);
            TextResponse = tc.Request;
            MediaResponse = string.Empty;
            NextPossibleTextRequests = AddRC(tc.NextResponses);
        }


        public MainController(EncyclopediaController ec, TestController tc, string keyWordToSEncyclopedya, string keyWordToSTest, string keyWordForReturn, string textInMenu, string mediaInMenu)
        {
            this.ec = ec;
            this.tc = tc;
            this.keyWordToSEncyclopedya = keyWordToSEncyclopedya;
            this.keyWordToSTest = keyWordToSTest;
            this.keyWordForReturn = keyWordForReturn;
            this.textInMenu = textInMenu;
            this.mediaInMenu = mediaInMenu;
        }
        public void ProcessingRequest(string request)
        {
            if (isFirstRequest) { PrepareMenu(); isFirstRequest = false; }
            else
            {

                if (request == keyWordForReturn)
                {
                    PrepareMenu();
                }
                else if (ec.EncyclopediaActivated == false && tc.TestIsStarted == false)
                {
                    if (request == keyWordToSEncyclopedya)
                    {
                        PrepareEnc(request);
                    }
                    else if (request == keyWordToSTest)
                    {
                        PrepareTest(request);
                    }
                }
                else if (ec.EncyclopediaActivated)
                {
                    PrepareEnc(request);
                }
                else if (tc.TestIsStarted)
                {
                    PrepareTest(request);
                }
            }

        }
    }
}
