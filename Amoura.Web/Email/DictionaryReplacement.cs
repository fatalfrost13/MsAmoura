using System;
using System.Collections.Generic;
using System.Linq;

namespace Amoura.Web.Email
{
    using System.Collections.Specialized;
    using Amoura.Web.Data;
    using Amoura.Web.Models;
    using umbraco.NodeFactory;

    public static class DictionaryReplacement
    {

        public static ListDictionary RecoverPassword(AccountModel model)
        {
            var dateNow = DateTime.Now;
            var replacements = new ListDictionary
                    {
                        { "&lt;&lt;Date&gt;&gt;", dateNow.ToString("MMMM d, yyyy") },
                        { "&lt;&lt;DateTime&gt;&gt;", dateNow.ToString("MMMM d, yyyy h:mm tt") },
                        { "&lt;&lt;FirstName&gt;&gt;", model.FirstName ?? string.Empty },
                        { "&lt;&lt;LastName&gt;&gt;", model.LastName ?? string.Empty },
                        { "&lt;&lt;Email&gt;&gt;", model.Email ?? string.Empty },
                        { "&lt;&lt;Password&gt;&gt;", model.Password ?? string.Empty }
                    };
            return replacements;
        }

        public static ListDictionary NewMember(AccountModel model)
        {
            var dateNow = DateTime.Now;
            var replacements = new ListDictionary
                    {
                        { "&lt;&lt;Date&gt;&gt;", dateNow.ToString("MMMM d, yyyy") },
                        { "&lt;&lt;DateTime&gt;&gt;", dateNow.ToString("MMMM d, yyyy h:mm tt") },
                        { "&lt;&lt;FirstName&gt;&gt;", model.FirstName ?? string.Empty },
                        { "&lt;&lt;LastName&gt;&gt;", model.LastName ?? string.Empty },
                        { "&lt;&lt;Email&gt;&gt;", model.Email ?? string.Empty },
                        { "&lt;&lt;Bio&gt;&gt;", model.Bio ?? string.Empty }
                    };
            return replacements;
        }
    }
}