using System.Collections.Generic;

namespace motw{
    public class Player{
        public ulong id { get; }
        public string name { get; }
        public string charName { get; }
        public Dictionary<string, int> ratings { get; }
        public int holds { get; set; }
        public string hunterClass { get; }
        public Dictionary<string, string> moves { get; }

        public Player(ulong _id, string _name, string _charName, Dictionary<string, int> _ratings, int _holds, string _hunterClass, Dictionary<string, string> _moves){
            id = _id;
            name = _name;
            charName = _charName;
            ratings = _ratings;
            holds = _holds;
            hunterClass = _hunterClass;
            moves = _moves;
        }
    }
}