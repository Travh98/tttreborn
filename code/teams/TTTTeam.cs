using System;
using System.Collections.Generic;

using Sandbox;

using TTTReborn.Globals;
using TTTReborn.Player;

namespace TTTReborn.Teams
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TeamAttribute : LibraryAttribute
    {
        public TeamAttribute(string name) : base(name)
        {

        }
    }

    public abstract class TTTTeam
    {
        public readonly string Name;

        public abstract Color Color { get; }

        public readonly List<TTTPlayer> Members = new();

        public static TTTTeam Instance;

        public static Dictionary<string, TTTTeam> Teams = new();

        public TTTTeam()
        {
            Name = Utils.GetTypeName(GetType());

            Instance = this;
            Teams[Name] = this;
        }
    }

    public static class TeamFunctions
    {
        public static TTTTeam GetTeam(string teamname)
        {
            if (teamname == null)
            {
                return null;
            }

            if (!TTTTeam.Teams.TryGetValue(teamname, out TTTTeam team))
            {
                team = Utils.GetObjectByType<TTTTeam>(Utils.GetTypeByName<TTTTeam>(teamname));
            }

            return team;
        }

        public static TTTTeam GetTeamByType(Type teamType)
        {
            foreach (TTTTeam team in TTTTeam.Teams.Values)
            {
                if (team.GetType() == teamType)
                {
                    return team;
                }
            }

            return null;
        }
    }
}
