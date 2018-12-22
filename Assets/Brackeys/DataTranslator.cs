using UnityEngine;
using System;

public class DataTranslator : MonoBehaviour
{

    public static string ValuesToData(string data,int kills, int deaths)
    {
        return data + "" + kills + "" + deaths;
    }

    public static float DataToValue(string data, int returnOption)
    {
        string[] pieces = data.Split('¬');
        string nickname = pieces[0];
        if (pieces.Length > 0)
        {
            string[] numericPieces=pieces[1].Split('°');
            switch (returnOption)
            {
                case 0://COLORES
                    float r = Convert.ToSingle(numericPieces[0]);
                    return r;

                case 1://COLORES
                    float g = Convert.ToSingle(numericPieces[1]);
                    return g;

                case 2://COLORES
                    float b = Convert.ToSingle(numericPieces[2]);
                    return b;

                case 3:
                    if (numericPieces.Length > 2)
                    {
                        string[] killDeathString = numericPieces[3].Split('^');
                        float killFloat = Convert.ToSingle(killDeathString[0]);
                        return killFloat;
                        
                    }
                    break;
                case 4:
                    if (numericPieces.Length > 2)
                    {
                        string[] killDeathString = numericPieces[3].Split('^');
                        float deathFloat = Convert.ToSingle(killDeathString[1]);
                        return deathFloat;
                    }
                    break; 
            }
        }
        return 0;
    }

    public static string DataToName(string _data)
    {
        string[] _dataPieces = _data.Split('¬');
        return _dataPieces[0];
    }

}
    /*
    public static int kills;
    public static int deaths;

    public static string ValuesToData(string data, int kills, int deaths)
    {
        return  data+"°"+kills +"^"+ deaths;
    }

    public static void DataToValues(string _data)
    {
        //NICKNAME
        Debug.Log("Data received from DATA TRANSLATOR:   "+_data);
        string[] pieces = _data.Split('¬');
        /*string _nickName = pieces[0];
        SetNickName(_nickName);
        Debug.Log("DATA TRANSLATOR: Current Nickname: " + _nickName);*/

       /* if (pieces.Length > 0)
        {
            //COLOR
            
            string[] colorPieces = pieces[1].Split('°');
            /*float _a, _b, _c;
            _a = Convert.ToSingle(colorPieces[0]);
            _b = Convert.ToSingle(colorPieces[1]);
            _c = Convert.ToSingle(colorPieces[2]);

            SetColor(_a, _b, _c);

            Debug.Log("Colors for player are R: " + _a + "   G: " + _b + "   B: " + _c + "   ");
            */
            //PLAYERSCORE
           /* if (colorPieces.Length > 2)
            {
                string[] scorePieces = colorPieces[3].Split('^');
                int _kills = Convert.ToInt32(scorePieces[1]);
                int _deaths = Convert.ToInt32(scorePieces[2]);
                SetScore(_kills, _deaths);
            }
        }
    }

    private static void SetScore(int _kills, int _deaths)
    {
        kills = _kills;
        deaths = _deaths;
    }

    private static string DataToValue(string _data, int _kills, int _deaths)
    {
        //DE NUMEROS A STRING
        string data = _data +"^"+ _kills +"^"+ _deaths;
        return data;
    }


}*/