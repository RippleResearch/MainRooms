using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController
{
    public List<Color> AllColors, colors1, beachColors, palettable;
    List<List<Color>> ListAllColors;

    public ColorController() {
        GenerateColors();
    }

    public void GenerateColors() {
        colors1 = new List<Color> {
            HexToRGB("EDD4B2"),
            HexToRGB("d0a98f"),
            HexToRGB("4d243d"),
            HexToRGB("cac2b5"),
            HexToRGB("ecdcc9")
        };

        beachColors = new List<Color> {
            HexToRGB("ED6A5A"),
            HexToRGB("F4F1BB"),
            HexToRGB("9BC1BC"),
            HexToRGB("E6EBE0"),
            HexToRGB("36C9C6")
        };

        palettable = new List<Color> {
            HexToRGB("C4D6B0"),
            HexToRGB("477998"),
            HexToRGB("291F1E"),
            HexToRGB("F64740"),
            HexToRGB("A3333D")
        };

        AllColors = new List<Color> {
            HexToRGB("EDD4B2"),
            HexToRGB("d0a98f"),
            HexToRGB("4d243d"),
            HexToRGB("cac2b5"),
            HexToRGB("ecdcc9"),
            HexToRGB("ED6A5A"),
            HexToRGB("F4F1BB"),
            HexToRGB("9BC1BC"),
            HexToRGB("E6EBE0"),
            HexToRGB("36C9C6"),
            HexToRGB("C4D6B0"),
            HexToRGB("477998"),
            HexToRGB("291F1E"),
            HexToRGB("F64740"),
            HexToRGB("A3333D")
        };

        ListAllColors = new List<List<Color>> {
                colors1,
                beachColors,
                palettable
          };
    }

    public KeyValuePair<string, List<Color>> HexColorAndPair(KeyValuePair<string, List<string>> colors) {
        List<Color> col = new List<Color>();
        foreach(string hex in colors.Value) {
            col.Add(HexToRGB(hex));
        }
        return new KeyValuePair<string, List<Color>>(colors.Key, col);
    }

    public List<Color> HexListToColor(List<string> hexValues) {
        List<Color> result = new List<Color>();
        foreach (string hexValue in hexValues) {
            result.Add(HexToRGB(hexValue));
        }
        return result;
    }
    public Color HexToRGB(string hexValue) {
        // Remove any leading "#" if present
        if (hexValue.StartsWith("#"))
            hexValue = hexValue.Substring(1);

        // Convert the hexadecimal value to integer
        int hex = Convert.ToInt32(hexValue, 16);

        // Extract the RGB components
        int red = (hex >> 16) & 0xFF;
        int green = (hex >> 8) & 0xFF;
        int blue = hex & 0xFF;

        return new Color(red / 255f, green / 255f, blue / 255f); // Unity needs number to be between 0 and 1
    }

    /// <summary>
    /// Returns a Lsit of unique colors
    /// </summary>
    /// <returns></returns>
    public List<Color> RandomNumberOfColors() {
        List<Color> used = new List<Color>();
        //Min number of colors is three
        int numOfColors = UnityEngine.Random.Range(3, AllColors.Count);
        
        for (int i = 0; i < numOfColors; i++) {
            Color color;
            do {
                List<Color> palate = ListAllColors[UnityEngine.Random.Range(0, ListAllColors.Count)];
                color = palate[UnityEngine.Random.Range(0, palate.Count)];
            } while (used.Contains(color));
            
            used.Add(color);
        }
        
        return used;
    }


    public List<Color> RandomOddNumberOfColors() {
        List<Color> used = new List<Color>();
        //Min number of colors is three
        int numOfColors = UnityEngine.Random.Range(3, AllColors.Count);
        numOfColors = numOfColors % 2 == 0 ? numOfColors - 1: numOfColors;

        for (int i = 0; i < numOfColors; i++) {
            Color color;
            do {
                List<Color> palate = ListAllColors[UnityEngine.Random.Range(0, ListAllColors.Count)];
                color = palate[UnityEngine.Random.Range(0, palate.Count)];
            } while (used.Contains(color));

            used.Add(color);
        }

        return used;
    }
}
