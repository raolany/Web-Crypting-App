using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace WebEncryptingSystem.Models
{
    public class LabsModel
    {
        private static LabsModel instance;
        private static IEnumerable<string> qwe;

        private LabsModel() { }

        public static LabsModel Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new LabsModel();
                }

                return instance;
            }
        }
    }
}