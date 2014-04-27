using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using McSntt.Views.UserControls;

namespace McSntt.Models
{
    public class TeamList
    {
        private static TeamList _instance;
        private TeamList() {}
        public static TeamList Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TeamList();
                }
                return _instance;
            }
        }

        public List<Team> Teams;
    }
}
