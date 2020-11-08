using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace MiraInterop
{
    public class MiraClient
    {
        #region Construction

        public MiraClient
            (
                MiraConfiguration configuration
            )
        {
            _configuration = configuration;
        }

        #endregion

        #region Private members

        private MiraConfiguration _configuration;

        #endregion

        #region Public methods

        public RestRequest CreateRequest()
        {
            RestRequest result = new RestRequest
                (
                    _configuration.Resource,
                    Method.GET
                );

            result.AddParameter("key", _configuration.Key);
            result.AddParameter("option", _configuration.Option);

            return result;
        }

        #endregion
    }
}
