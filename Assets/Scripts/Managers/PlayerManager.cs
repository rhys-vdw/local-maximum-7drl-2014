using UnityEngine;
using UnityObjectRetrieval;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;

public class PlayerManager : MonoBehaviour
{
    Player[] m_Players = new Player[4];

    public void RegisterPlayer( int number, Player player )
    {
        m_Players[ number ] = player;
    }

    public Player GetPlayer( int number )
    {
        return m_Players[ number ];
    }

    public IEnumerable<Player> Players
    {
        get { return m_Players; }
    }
}
