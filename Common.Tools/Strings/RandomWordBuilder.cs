using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Common.Tools.Strings
{
    public class RandomWordBuilder
    {
        private ArrayList _RandomSentences;

        private Random _RandomNumberGenerator;

        #region "Constructors"

        public RandomWordBuilder()
        {
            _RandomNumberGenerator = new Random();

            _RandomSentences = new ArrayList();

            //Initialize the Random sentences array with a bunch of random sentences.
            _RandomSentences.Add("Most of us would admit to finding our working day slightly tedious at times.");
            _RandomSentences.Add("But spare a thought for Keith Jackson");
            _RandomSentences.Add("The married father-of-one has perhaps the most boring job in the world - watching paint dry.");
            _RandomSentences.Add("For more than 30 years, assessing the drying time of industrial paint has been part of Mr Jackson's working life.");
            _RandomSentences.Add("The highlight of his day is simply touching the paint to assess it's tackiness.");
            _RandomSentences.Add("Although he admits his job is seen as a bit of a joke and can be slightly tedious at times, the 57-year-old has never tired of the task.");
            _RandomSentences.Add("\"People do laugh and find it amusing when I tell them what I do,' Mr Jackson said yesterday\"");
            _RandomSentences.Add("\"It could be described as the most boring job in the world, but it is a very important one.\"");
            _RandomSentences.Add("\"We supply paint to a variety of industries and for our customers it is very important that they can cover their products with paint that dries quickly.\"");
            _RandomSentences.Add("\"For example, we make the paint for the floors and walls of stations for the London Underground.\"");
            _RandomSentences.Add("\"They can't afford to shut to passengers for long periods so the painting can only be done between 3am and 5am.\"");
            _RandomSentences.Add("\"Once the paint is on the floor it has to dry hard and fast enough for people to be able to walk on in time for when the station re-opens in the morning.\"");
            _RandomSentences.Add("Mr Jackson, from Llandegla, North Wales, whose official title is technical manager, has worked for industrial paint manufacturers AquaTec Coatings for the past four years.");
            _RandomSentences.Add("However, he has been in the paint industry since he left school as a 16-year-old.");
            _RandomSentences.Add("Mr Jackson refused to reveal exactly how much he earned, but said watching paint dry paid fairly well.");
            _RandomSentences.Add("\"Watching paint dry sounds quite easy, but it can be stressful at times\"");
            _RandomSentences.Add("\"I put the paint on pieces of cardboard and literally time how long they take to dry with a stop watch.\"");
            _RandomSentences.Add("\"Once we have done that the paints then go through a whole host of other tests, such as accelerated weather testing, when they are sprayed with salt water to see how they fare.\"");
            _RandomSentences.Add("Mr Jackson's boss, Anthony Kershaw, said he was doing one of the most important jobs in the company, which is based in Wrexham, North Wales.");
            _RandomSentences.Add("\"It sounds very boring but it is very important. If our paint didn't dry quickly we wouldn't have any customers.\"");

        }

        #endregion

        #region "Properties"

        private int AvailableRandomSentencesCount
        {
            get { return _RandomSentences.Count; }
        }

        #endregion

        #region "Public Methods"


        public string GetRandomSentences(int NumberOfSentences)
        {
            string strRandomSentences = string.Empty;

            for (int i = 1; i <= NumberOfSentences; i++)
            {
                strRandomSentences += GetRandomSentence();
            }

            return strRandomSentences.Trim(); ;
        }

        public string GetRandomSentence()
        {
            int intRandomSentenceIndex = GetRandomNumber(AvailableRandomSentencesCount);

            string strRandomSentence = Convert.ToString(_RandomSentences[intRandomSentenceIndex]);

            return strRandomSentence.Trim();
        }



        public string GetRandomWord()
        {
            string strRandomSentence = GetRandomSentence();

            string[] aWords = strRandomSentence.Split(' ');

            int intRandomWordIndex = GetRandomNumber(aWords.Length);

            string strRandomWord = aWords[intRandomWordIndex];

            return strRandomWord.Trim();
        }
        #endregion



        private int GetRandomNumber(int Max)
        {
            int intNumber = _RandomNumberGenerator.Next(AvailableRandomSentencesCount);
            return intNumber;
        }

    }
}
