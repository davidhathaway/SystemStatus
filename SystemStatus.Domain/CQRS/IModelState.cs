using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemStatus.Domain
{
    public interface IModelState
    {
        void AddError(string key, string errorMessage);
        bool IsValid { get; }

        bool DoesKeyExist(string key);
    }
}
