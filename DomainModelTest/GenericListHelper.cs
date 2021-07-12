using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModelTest
{
    public static class GenericListHelper
    {
        private static Random rand = new Random();

        public static Queue<T> ToQueue<T>(List<T> inputList, bool shuffle = false)
        {
            var inputListCopy = inputList.ToList();
            var outputQueue = new Queue<T>();

            while (inputListCopy.Count > 0)
            {
                int index = shuffle ? rand.Next(0, inputListCopy.Count - 1) : 0;
                var element = inputListCopy.ElementAt(index);

                outputQueue.Enqueue(element);
                inputListCopy.Remove(element);
            }

            return outputQueue;
        }
    }
}
