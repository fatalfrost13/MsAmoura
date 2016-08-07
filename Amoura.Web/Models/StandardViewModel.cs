using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.NodeFactory;

namespace Amoura.Web.Models
{
    public class StandardViewModel
    {
        public bool Initialized { get; set; } = false;
        public Node PageNode { get; set; }
    }
}