using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVM_SMO
{
    public interface IFileManager
    {
        string ReadLine();

        void ResetReader();
    }
}
