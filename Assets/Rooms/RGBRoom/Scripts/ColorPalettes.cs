using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity;
using UnityEngine;
using Random = System.Random;
using System.Drawing.Imaging;

public class ColorPalettes {
    private static readonly Random random = new Random();
    private const int MIN_COLORS = 3;
    private const int MAX_COLORS = 13;

    //Correct line
    public static List<Dictionary<string, List<string>>> colorMap = new List<Dictionary<string, List<string>>>();
    private static List<Dictionary<string, List<string>>> cbColorMap = new List<Dictionary<string, List<string>>>();


    private static Dictionary<string, List<string>> colorMapAll = new Dictionary<string, List<string>>();
    private static Dictionary<string, List<string>> cbColorMapAll = new Dictionary<string, List<string>>();

    private static List<List<KeyValuePair<string, List<string>>>> colorList = new List<List<KeyValuePair<string, List<string>>>>();
    private static List<List<KeyValuePair<string, List<string>>>> cbColorList = new List<List<KeyValuePair<string, List<string>>>>();

    private static List<KeyValuePair<string, List<string>>> colorListAll = new List<KeyValuePair<string, List<string>>>();
    private static List<KeyValuePair<string, List<string>>> cbColorListAll = new List<KeyValuePair<string, List<string>>>();
    //-----------------------------------Correct ^^^

    static ColorPalettes() {
        cbColorMap.Add(new Dictionary<string, List<string>> {
    {"Viridis (3)" , new List<string>{"fde725","21918c","440154"}}
    });
        cbColorMap.Add(new Dictionary<string, List<string>> {
    // From https://davidmathlogic.com/colorblind/#%23D81B60-%231E88E5-%23FFC107-%23004D40
    {"Nichols (4)" , new List<string>{"d81b60","1e88e5","ffc107","004d40"}},
    // From https://waldyrious.net/viridis-palette-generator/
    {"Viridis (4)" , new List<string>{"fde725","35b779","31688e","440154"}},
    {"Inferno (4)", new List<string>{"fcffa4","ed6925","781c6d","000004"}},
    {"Magma (4)", new List<string>{"fcfdbf","f1605d","721f81","000004"}},
    {"Plasma (4)", new List<string>{"f0f921","ed7953","9c179e","0d0887"}}
});
        cbColorMap.Add(new Dictionary<string, List<string>> {
    // From https://davidmathlogic.com/colorblind/#%23D81B60-%231E88E5-%23FFC107-%23004D40
    {"IBM (5)" , new List<string>{"648FFF","785EF0","DC267F","FE6100","FFB000"}},
    // From https://personal.sron.nl/~pault/#sec:qualitative
    {"Tol High Contrast (5)" , new List<string>{"ffffff","ddaa33","bb5566","004488","000000"}},
    // Next 4 from https://waldyrious.net/viridis-palette-generator/
    {"Viridis (5)" , new List<string>{"fde725","5ec962","21918c","3b528b","440154"}},
    {"Inferno (5)", new List<string>{"fcffa4","f98e09","bc3754","57106e","000004"}},
    {"Magma (5)", new List<string>{"fcfdbf","fc8961","b73779","51127c","000004"}},
    {"Plasma (5)", new List<string>{"f0f921","f89540","cc4778","7e03a8","0d0887"}}
});
        cbColorMap.Add(new Dictionary<string, List<string>> {
    // From https://personal.sron.nl/~pault/#sec:qualitative
    {"Tol Pale (6)" , new List<string>{"bbccee","cceeff","ccddaa","eeeebb","ffcccc","dddddd"}},
    // From https://personal.sron.nl/~pault/#sec:qualitative
    {"Tol Dark (6)" , new List<string>{"222255","225555","225522","666633","663333","555555"}},
    // Next 4 from https://waldyrious.net/viridis-palette-generator/
    {"Viridis (6)", new List<string>{"fde725", "7ad151", "22a884", "2a788e", "414487","440154"}},
    {"Inferno (6)", new List<string>{"fcffa4","fca50a","dd513a","932667","420a68","000004"}},
    {"Magma (6)", new List<string>{"fcfdbf","fe9f6d","de4968","8c2981","3b0f70","000004"}},
    {"Plasma (6)", new List<string>{"f0f921","fca636","e16462","b12a90","6a00a8","0d0887"}}
});
        cbColorMap.Add(new Dictionary<string, List<string>> {
    // From https://personal.sron.nl/~pault/#sec:qualitative
    {"Tol Bright (7)" , new List<string>{"4477aa","66ccee","228833","ccbb44","ee6677","aa3377","bbbbbb"}},
    // From https://personal.sron.nl/~pault/#sec:qualitative
    {"Tol Vibrant (7)" , new List<string>{"0077BB","33bbee","009988","ee7733","cc3311","ee3377","bbbbbb"}},
    // Next 4 from https://waldyrious.net/viridis-palette-generator/
    {"Viridis (7)", new List<string>{"fde725", "90d743", "35b779", "21918c", "31688e","443983" ,"440154"}},
    {"Inferno (7)", new List<string>{"fcffa4","fbb61a","ed6925","bc3754","781c6d","320a5e","000004"}},
    {"Magma (7)", new List<string>{"fcfdbf","feb078","f1605d","b73779","721f81","2c115f","000004"}},
    {"Plasma (7)", new List<string>{"f0f921","fdb42f","ed7953","cc4778","9c179e","5c01a6","0d0887"}}
});
        cbColorMap.Add(new Dictionary<string, List<string>> {
    // from https://davidmathlogic.com/colorblind/#%23D81B60-%231E88E5-%23FFC107-%23004D40
    // and also https://mikemol.github.io/technique/colorblind/2018/02/11/color-safe-palette.html
    {"Wong Ito (8)" , new List<string>{"000000","E69F00","56B4E9","009E73","F0E442","0072B2","D55E00","CC79A7"}},
    // From https://personal.sron.nl/~pault/#sec:qualitative
    {"Tol Medium Contrast (8)", new List<string>{"ffffff", "eecc66", "ee99aa", "6699cc", "997700", "994455", "004488", "000000"}},
    // From https://personal.sron.nl/~pault/#sec:qualitative
    {"Tol Light (8)" , new List<string>{"77aadd","99ddff","44bb99","bbcc33","aaaa00","eedd88","ee8866","ffaabb"}},
    // Next 4 from https://waldyrious.net/viridis-palette-generator/
    {"Viridis (8)", new List<string>{"fde725", "a0da39", "4ac16d", "1fa187", "277f8e","365c8d" ,"46327e", "440154"}},
    {"Inferno (8)", new List<string>{"fcffa4", "fac228", "f57d15", "d44842", "9f2a63", "65156e", "280b53", "000004"}},
    {"Magma (8)", new List<string>{"fcfdbf","febb81", "f8765c", "d3436e", "982d80", "5f187f", "221150", "000004"}},
    {"Plasma (8)", new List<string>{"f0f921", "febd2a", "f48849", "db5c68", "b83289", "8b0aa5", "5302a3", "0d0887"}}
});
        cbColorMap.Add(new Dictionary<string, List<string>> {
    // From https://personal.sron.nl/~pault/#sec:qualitative
    {"Tol Muted (9)" , new List<string>{"332288","88ccee","44aa99","117733","999933","ddcc77","cc6677","882255","aa4499"}},
    // From https://personal.sron.nl/~pault/#sec:qualitative
    {"Tol Light (9)" , new List<string>{"77aadd","99ddff","44bb99","bbcc33","aaaa00","eedd88","ee8866","ffaabb","dddddd"}},
    // Next 4 from https://waldyrious.net/viridis-palette-generator/
    {"Viridis (9)", new List<string>{"fde725", "addc30", "5ec962", "28ae80", "21918c","2c728e" ,"3b528b","472d7b", "440154"}},
    {"Inferno (9)", new List<string>{"fcffa4", "f9cb35", "f98e09", "e45a31", "bc3754", "8a226a", "57106e", "210c4a", "000004"}},
    {"Magma (9)", new List<string>{"fcfdbf", "fec488", "fc8961", "e75263", "b73779", "832681", "51127c", "1d1147", "000004"}},
    {"Plasma (9)", new List<string>{"f0f921", "fdc527", "f89540", "e66c5c", "cc4778", "aa2395", "7e03a8", "4c02a1", "0d0887"}}
});
        cbColorMap.Add(new Dictionary<string, List<string>> {
    // Next 3 from https://waldyrious.net/viridis-palette-generator/
    {"Viridis (10)", new List<string>{"fde725", "b5de2b", "6ece58","35b779","1f9e89","26828e","31688e","3e4989","482878","440154"}},
    {"Plasma (10)", new List<string>{"f0f921", "fdca26", "fb9f3a", "ed7953", "d8576b", "bd3786", "9c179e", "7201a8", "46039f", "0d0887"}},
    {"Magma (10)", new List<string>{"fcfdbf", "feca8d", "fd9668", "f1605d", "cd4071", "9e2f7f", "721f81", "440f76", "180f3d", "000004"}}

});
        cbColorMap.Add(new Dictionary<string, List<string>> {
    // Next 3 from https://waldyrious.net/viridis-palette-generator/
    {"Inferno (11)", new List<string>{"fcffa4", "f6d746", "fca50a", "f37819", "dd513a", "bc3754", "932667", "6a176e", "420a68", "160b39", "000004"}},
    {"Plasma (11)", new List<string>{"f0f921", "fcce25", "fca636", "f2844b", "e16462", "cc4778", "b12a90", "8f0da4", "6a00a8", "41049d", "0d0887"}},
    {"Viridis (11)", new List<string>{"fde725", "bddf26", "7ad151", "44bf70", "22a884", "21918c", "2a788e", "355f8d", "414487", "482475", "440154"}}
});
        cbColorMap.Add(new Dictionary<string, List<string>> {
     {"Viridis (12)" , new List<string>{"fde725","c2df23","86d549","52c569","2ab07f","1e9b8a","25858e","2d708e","38588c","433e85","482173","440154" }}
});
        cbColorMap.Add(new Dictionary<string, List<string>> {
    {"Viridis (13)", new List<string>{"fde725","c8e020","90d743","5ec962","35b779","20a486","21918c","287c8e","31688e","3b528b","443983","481f70","440154" }}
});

        colorMap.Add(new Dictionary<string, List<string>> {
    // Next 2 From https://digitalsynopsis.com/design/tri-color-palettes-combinations-schemes/
    {"RYG (3)" , new List<string>{"ff1700","ffa600","4d6910"}},
    {"Red To Tan (3)" , new List<string>{"b72818","bc6022","e8c599"}},
    // Next several From https://digitalsynopsis.com/design/color-palettes-schemes-combos-inspiration/
    {"Fairway Green (3)" , new List<string>{"1d3c26","578f53","EADFCE"}},
    {"Copper Tones (3)" , new List<string>{"894329","12727d","fcb886"}},
    {"Desert Plain (3)" , new List<string>{"304655","b59283","e5d4cd"}},
    {"Summer Vibe (3)" , new List<string>{"1998d3","41A592","f6ce4b"}},
    {"Beach Hut (3)" , new List<string>{"d05146","dbc7b5","6091ab"}},
    // From Hope College materials.
    {"Hope College (3)", new List<string>{"F46A1F", "f7e654", "002244"}},
    // I created based on images from the RFQ
    {"GFIAA Water (3)", new List<string>{"5e9fd6","dbd7c1","b7d2e9"}},
    {"GFIAA Water 2 (3)", new List<string>{"4c9cd4","9bb6ce","ddd5b9"}},
    {"GFIAA Dunes (3)", new List<string>{"d5bf92","ddd5c3","8d715f"}},
    {"GFIAA Forest (3)", new List<string>{"c0cd8e","ded5c3","374c02"}},
    {"GFIAA Forest 2 (3)", new List<string>{"ddd5b9","7c782f","3a4a38"}},
});
        colorMap.Add(new Dictionary<string, List<string>> {
    {"Grayscale (4)" , new List<string>{"FFFFFF", "ABABAB", "555555", "000000"}},
    {"RGBY (4)", new List<string>{"FF0000","00FF00","0000FF","FFFF00"}},
    //From https://logosbynick.com/70s-color-palettes-with-hex/
    {"70s Mels Drive-In (4)" , new List<string>{"e1b10f", "e0cdd0", "fb9ab6", "448a9a"}},
    {"70s Yellos Beetle (4)" , new List<string>{"688052", "c4e2e2", "fa9584", "f9ae54"}},
    {"70s Retro Van (4)" , new List<string>{"344126", "f3b87b", "bb6c5d", "352921"}},
    {"70s Yellow Roses (4)" , new List<string>{"6f583d", "7096d2", "e0a23d", "505221"}},
    {"70s Sunflower (4)" , new List<string>{"65b0bb", "56432d", "e1dac7", "e1aa46"}},
    {"70s Blue Springs Bowling (4)" , new List<string>{"59a8a3", "9e3532", "eaded0", "2e465e"}},
    {"70s Motel 66 (4)" , new List<string>{"4dacb4", "dcdce0", "d3a06e", "e4d7b8"}},
    {"70s 24 Hour Bowling (4)" , new List<string>{"79faef", "064a5a", "f8b591", "f4473a"}},
    {"70s Sunny Day (4)" , new List<string>{"fca815", "923a21", "e4c5a7", "6b724e"}},
    {"70s Fashion (4)" , new List<string>{"742e12", "d28a2d", "bfbda2", "2f6c68"}},
    {"70s Groovy Shades (4)" , new List<string>{"e45356", "7da75e", "fae679", "4182bc"}},
    // From https://www.schemecolor.com/vintage-print-color-scheme.php
    {"Vintage Print (4)" , new List<string>{"224F55", "9D8342", "CDBF96", "7A8A6F"}},
    // From https://www.schemecolor.com/pastel-poster-retro.php
    {"Pastel Poster (Retro) (4)" , new List<string>{"A6C198", "FEF4C1", "FED998", "E38F95"}},
    //From https://www.schemecolor.com/stroll-in-fall.php
    {"Stroll in Fall (4)" , new List<string>{"D15A51", "EA952D", "ECC872", "6C6D29"}},
    // From https://www.shutterstock.com/blog/25-free-retro-color-palettes (#24)
    {"Pin-Up and Piers (4)" , new List<string>{"b4d3aa", "84d2de", "ea8f3c", "b24629"}},
    // From https://colorhunt.co/palette/9ede73f7ea00e48900be0000
    {"Neonish (4)" , new List<string>{"9EDE73","F7EA00","E48900","BE0000"}},
    // From https://colorhunt.co/palette/c85c5cf9975dfbd148b2ea70
    {"Muted Rainbow (4)" , new List<string>{"C85C5C","F9975D","FBD148","B2EA70"}},
    // From https://colorhunt.co/palette/064635519259f0bb62f4eea9
    {"Greean and Yellow (4)" , new List<string>{"064635", "519259","F0BB62","F4EEA9"}},
    // From https://colorswall.com/palette/38
    {"Google Logo (4)" , new List<string>{"4285f4","ea4335","fbbc05","34a853"}},
    // From https://colorswall.com/palette/121
    {"Lego Logo (4)" , new List<string>{"d01012","f6ec36","000000","ffffff"}},
    // From https://colorswall.com/palette/72
    {"Microsoft Logo (4)" , new List<string>{"f25022","7fba00","00a4ef","ffb900"}},
    // Next bunch from Hope public affairs and marketing stuff.
    {"Hope College A (4)" , new List<string>{"F7E654","F46A1F","00685B","002244"}},
    {"Hope College B (4)" , new List<string>{"F46A1F","BED600","00685B","002244"}},
    {"Hope College Dark (4)" , new List<string>{"F46A1F","91420E","002244","00685B"}},
    {"Hope College C (4)" , new List<string>{"F46A1F","002244","00685B","00B0CA"}},
    {"Hope College D (4)" , new List<string>{"F46A1F","F0AB00","BED600","002244"}},
    {"Hope Orange (4)", new List<string>{"91420E","F46A1F",  "F0AB00", "F7E654"}},
    {"Hope Orange and Green (4)", new List<string>{"F46A1F", "F0AB00","BED600",  "00685B"}},
    {"Hope Blue (4)" , new List<string>{"002244","5482AB","00B0CA","BBE7E6"}},
    {"Hope Blue and Green (4)" , new List<string>{"002244","00685B","00B0CA","BED600"}},
    {"Hope College With Neutrals (4)" , new List<string>{"F46A1F","002244","000000","4D4F53"}},
    // From https://replay404.com/strong-colors-palette-of-yellow-and-orange/
    {"Strong Colors (4)" , new List<string>{"523024", "55664B", "D69745", "BF3C30"}},
    // I created based on images from the RFQ
    {"GFIAA Water (4)", new List<string>{"4c9cd4","9bb6ce","6f899e","ddd5b9"}},
    {"GFIAA Dunes (4)", new List<string>{"ddd5b9","bca885","967e6d","604c35"}},
    {"GFIAA (4)", new List<string>{"dadedd","3a4a39","7c782f","604c35"}},
});

        // ---------------------------------------------------------------------------------------
        // 5
        colorMap.Add(new Dictionary<string, List<string>> {
    {"Grayscale (5)" , new List<string>{"FFFFFF", "C0C0C0", "808080", "404040", "000000"}},
    // from https://www.color-hex.com/color-palette/61294
    {"70s Retro Stripes (5)" , new List<string>{"75C8AE", "5A3D2B", "FFECB4", "E5771E", "F4A127"}},
    // next 7 from https://looka.com/blog/vintage-color-palettes/
    {"Vintage DYFAM (5)" , new List<string>{"ef694d", "dddb76", "e4f2ef", "388895", "42403f"}},
    {"Vintage Telly (5)" , new List<string>{"161514", "b85c3c", "e5af7d", "fff6e9", "9bceb5"}},
    {"Vintage Shaka (5)" , new List<string>{"020001", "001c4b", "8cbeea", "9eb291", "ae9068"}},
    {"Vintage Rodeo Drive (5)" , new List<string>{"f4debe", "bfa082", "847b45", "4d675a", "f5aa5f"}},
    {"Vintage Home Sweet Home (5)" , new List<string>{"282121", "8b5939", "eacc81", "dae0da", "a0ddfb"}},
    {"Vintage Arcade (5)" , new List<string>{"f0f14e", "6ded8a", "1645f5", "ff5f85", "ed3833"}},
    {"Vintage Home (5)" , new List<string>{"669fb2", "dadfe1", "e8b877", "dd802c", "3e2a20"}},
    // From https://www.schemecolor.com/5-shades-of-gray-colors.php
    {"Just Grays (5)" , new List<string>{"D4D4D4", "B4B4B4", "909090", "636363", "494848"}},
    // From https://www.schemecolor.com/expressing-doubt.php
    {"Expressing Doubt (5)" , new List<string>{"7F8970", "CEB288", "955236", "284441", "B79552"}},
    // From https://www.schemecolor.com/retro-brown-orange-blue.php
    {"Retro Brown, Orange, Blue (5)" , new List<string>{"815F41", "6B4524", "D89058", "DBAF82", "526678"}},
    // From https://www.schemecolor.com/naturally-retro.php
    {"Naturally Retro (5)" , new List<string>{"5E4C4F", "5E7480", "C09668", "7F8662", "59614F"}},
    // From https://www.schemecolor.com/retro-game.php
    {"Retro Game (5)" , new List<string>{"4FAF44", "F6EB14", "FF9526", "EF4423", "2A3492"}},
    //https://www.schemecolor.com/vintage-country.php
    {"Vintage Country (5)" , new List<string>{"53BEB6", "F9DFC0", "F0B45E", "E79054", "B76450"}},
    // From https://www.schemecolor.com/fall-in-retro-colors.php
    {"Fall in Retro (5)" , new List<string>{"754F38", "AA5E47", "C48154", "DDA254", "4D7A5C"}},
    // From https://www.refactoringui.com/previews/building-your-color-palette
    {"Obnoxious Website (5)" , new List<string>{"173F5F","20639B","3CAEA3","F6D55C","ED553B"}},
    // From https://co.pinterest.com/pin/32158584826919694
    {"Five Color Rainbow (5)" , new List<string>{"ac92eb", "4fc1e8", "a0d568", "ffce54","ed5564"}},
    // From https://colorswall.com/palette/1191
    {"Colorswall random 666 (5)" , new List<string>{"4c5ba6","2cb1a2","6ee397","ec7299","d8d968"}},
    // From https://colorswall.com/palette/893
    {"Bedroom Relax (5)" , new List<string>{"658459","cac76c","c7aa7a","d4c6ab","ffffff"}},
    // From https://colorswall.com/palette/40134 (I removed 3 colors)
    {"CSS FF 6 (5)" , new List<string>{"cc4b37", "ffae00", "3adb76", "767676", "1779ba"}},
    // From https://colorswall.com/palette/2
    {"Bootstrap 3 (5)" , new List<string>{"337ab7", "5cb85c", "f0ad4e", "5bc0de", "d9534f"}},
    // Next 2 from https://loading.io/color/feature/
    {"Polaroid (5)" , new List<string>{"00a3e2", "1ba548","fdc800","f1860e","e41b13"}},
    {"Tableau (5)" , new List<string>{"ca1e32","eb7e24","c0c4c9","5c8c9c","3a5487"}},
    {"Happiness And Cyanide (5)" , new List<string>{"facd9e","389798","459448","f19d3b","923f2b"}},
    {"Hope College (5)" , new List<string>{"F46A1F","002244","F7E654","00685B","00B0CA"}},
    // Next 128 from https://mran.microsoft.com/snapshot/2020-04-29/web/packages/lisa/readme/README.html
    // (Actually from R based on that).
    {"JosefAlbers (5)" , new List<string>{"D77186","61A2DA","6CB7DA","b5b5b3","D75725"}},
    {"JosefAlbers_1 (5)" , new List<string>{"C00559","DE1F6C","F3A20D","F07A13","DE6716"}},
    {"GretchenAlbrecht (5)" , new List<string>{"171635","00225D","763262","CA7508","E9A621"}},
    {"BillyApple (5)" , new List<string>{"F24D98","813B7C","59D044","F3A002","F2F44D"}},
    {"PerArnoldi (5)" , new List<string>{"C2151B","2021A0","3547B3","E2C43F","E0DCDD"}},
    {"MiltonAvery (5)" , new List<string>{"F3C937","7B533E","BFA588","604847","552723"}},
    {"MiltonAvery_1 (5)" , new List<string>{"E2CACD","2E7CA8","F1C061","DA7338","741D13"}},
    {"HilmaafKlint (5)" , new List<string>{"D6CFC4","466CA6","D1AE45","87240E","040204"}},
    {"Jean-MichelBasquiat (5)" , new List<string>{"8CABD9","F6A7B8","F1EC7A","1D4D9F","F08838"}},
    {"Jean-MichelBasquiat_1 (5)" , new List<string>{"C11432","009ADA","66A64F","FDD10A","070707"}},
    {"MaxBeckmann (5)" , new List<string>{"4B3A51","A77A4B","ECC6A2","A43020","722D24"}},
    {"FernandoBotero (5)" , new List<string>{"99B6BD","B3A86A","ECC9A0","D4613E","BB9568"}},
    {"SandroBotticelli (5)" , new List<string>{"7A989A","849271","C1AE8D","CF9546","C67052"}},
    {"SandroBotticelli_1 (5)" , new List<string>{"272725","DDBD85","DA694F","A54A48","FDFFE5"}},
    {"PieterBruegel (5)" , new List<string>{"BFBED5","7F9086","A29A68","676A4F","A63C24"}},
    {"JackBush (5)" , new List<string>{"529DCB","ECA063","71BF50","F3CC4F","D46934"}},
    {"JackBush_1 (5)" , new List<string>{"A1D8B6","D2C48E","F45F40","F9AE8D","80B9CE"}},
    {"MaryCassatt (5)" , new List<string>{"1C5679","BBB592","CAC3B2","808C5C","5F4B3B"}},
    {"PaulCezanne (5)" , new List<string>{"8399B3","697A55","C4AA88","B68E52","8C5B28"}},
    {"MarcChagall (5)" , new List<string>{"3F6F76","69B7CE","C65840","F4CE4B","62496F"}},
    {"C M Coolidge (5)" , new List<string>{"204035","4A7169","BEB59C","735231","49271B"}},
    {"SalvadorDali (5)" , new List<string>{"40798C","bca455","bfb37f","805730","514A2E"}},
    {"SalvadorDali_1 (5)" , new List<string>{"9BC0CC","CAD8D8","D0CE9F","806641","534832"}},
    {"LeonardodaVinci (5)" , new List<string>{"C8B272","a88b4c","a0a584","697153","43362a"}},
    {"GeneDavis (5)" , new List<string>{"293757","568D4B","D5BB56","D26A1B","A41D1A"}},
    {"GiorgiodeChirico (5)" , new List<string>{"27403D","48725C","212412","F3E4C2","D88F2E"}},
    {"GiorgiodeChirico_1 (5)" , new List<string>{"2992BF","4CBED9","292C17","F9F6EF","F0742A"}},
    {"EdgarDegas (5)" , new List<string>{"BDB592","ACBBC5","9E8D3D","8C4F36","2C2D2C"}},
    {"RobertDelaunay (5)" , new List<string>{"4368B6","78A153","DEC23B","E4930A","C53211"}},
    {"RobertDelaunay_1 (5)" , new List<string>{"A4B7E1","B8B87A","EFDE80","EFBD37","A85E5E"}},
    {"CharlesDemuth (5)" , new List<string>{"e4af79","df9c41","af7231","923621","2D2A28"}},
    {"RichardDiebenkorn (5)" , new List<string>{"2677A5","639BC1","639BC1","90A74A","5D8722"}},
    {"OttoDix (5)" , new List<string>{"1E1D20","B66636","547A56","BDAE5B","515A7C"}},
    {"OttoDix_1 (5)" , new List<string>{"E0DBC8","C9BE90","76684B","CDAB7E","3C2B23"}},
    {"MarcelDuchamp (5)" , new List<string>{"d0cec2","7baa80","4b6b5e","bf9a41","980019"}},
    {"AlbrechtDurer (5)" , new List<string>{"657359","9AA582","8B775F","D7C9BE","F1E4DB"}},
    {"MaxErnst (5)" , new List<string>{"91323A","3A4960","D7C969","6D7345","554540"}},
    {"M C Escher (5)" , new List<string>{"C1395E","AEC17B","F0CA50","E07B42","89A7C2"}},
    {"PaulFeeley (5)" , new List<string>{"2C458D","E4DFD9","425B4F","EBAD30","BF2124"}},
    {"LorserFeitelson (5)" , new List<string>{"202221","661E2A","AB381B","EAD4A3","E3DED8"}},
    {"HelenFrankenthaler (5)" , new List<string>{"5D7342","D7AE04","ECD7B8","A58B8C","272727"}},
    {"LucianFreud (5)" , new List<string>{"e1d2bd","a77e5e","2d291d","85868b","83774d"}},
    {"TerryFrost (5)" , new List<string>{"EF5950","8D5A78","C66F26","FB6B22","DC2227"}},
    {"PaulGauguin (5)" , new List<string>{"21344F","8AAD05","E2CE1B","DF5D22","E17976"}},
    {"RupprechtGeiger (5)" , new List<string>{"FF62A9","F77177","FA9849","FE6E3A","FD5A35"}},
    {"HansHofmann (5)" , new List<string>{"1A6DED","2C7CE6","145CBF","162B3D","F9ECE4"}},
    {"KatsushikaHokusai (5)" , new List<string>{"1F284C","2D4472","6E6352","D9CCAC","ECE2C6"}},
    {"WinslowHomer (5)" , new List<string>{"A9944A","F2D9B3","725435","8E9DBF","BD483C"}},
    {"EdwardHopper (5)" , new List<string>{"67161C","3F6148","DBD3A4","A4804C","4B5F80"}},
    {"RobertIndiana (5)" , new List<string>{"2659D8","1C6FF3","5EBC4E","53A946","F24534"}},
    {"JamesJean (5)" , new List<string>{"51394E","F6DE7D","C8AF8A","658385","B04838"}},
    {"JasperJohns (5)" , new List<string>{"4B6892","F9E583","FED43F","F6BD28","BE4C46"}},
    {"FridaKahlo (5)" , new List<string>{"121510","6D8325","D6CFB7","E5AD4F","BD5630"}},
    {"WassilyKandinsky (5)" , new List<string>{"5D7388","A08F27","E5A729","4F4D1D","8AAE8A"}},
    {"WassilyKandinsky_1 (5)" , new List<string>{"d2981a","a53e1f","457277","8dcee2","8f657d"}},
    {"WassilyKandinsky_2 (5)" , new List<string>{"C13C53","DA73A8","4052BD","EFE96D","D85143"}},
    {"PaulKlee (5)" , new List<string>{"A7B3CD","E6DA9E","676155","CDB296","CCD7AD"}},
    {"PaulKlee_1 (5)" , new List<string>{"4F51FE","8C1E92","FF4E0B","CD2019","441C21"}},
    {"YvesKlein (5)" , new List<string>{"344CB9","1B288A","0F185B","D7C99A","F2E4C7"}},
    {"GustavKlimt (5)" , new List<string>{"4A5FAB","609F5C","E3C454","A27CBA","B85031"}},
    {"JeffKoons (5)" , new List<string>{"D6AABE","B69F7F","ECD9AD","76A9A2","A26775"}},
    {"LeeKrasner (5)" , new List<string>{"333333","D1B817","2A2996","B34325","C8CCC6"}},
    {"JacobLawrence (5)" , new List<string>{"614671","BE994A","C8B595","BD4335","8B3834"}},
    {"JacobLawrence_1 (5)" , new List<string>{"5E3194","9870B9","F1B02F","EA454C","CC0115"}},
    {"SolLeWitt (5)" , new List<string>{"0A71B6","F9C40A","190506","EB5432","EAF2F0"}},
    {"RoyLichtenstein (5)" , new List<string>{"3229ad","bc000e","e7cfb7","ffec04","090109"}},
    {"RoyLichtenstein_1 (5)" , new List<string>{"00020E","FFDE01","A5BAD6","F1C9C7","BD0304"}},
    {"RoyLichtenstein_2 (5)" , new List<string>{"c7991f","c63d33","23254c","e0c4ae","d5d0b2"}},
    {"KazimirMalevich (5)" , new List<string>{"151817","001A56","197C3F","D4A821","C74C25"}},
    {"EdouardManet (5)" , new List<string>{"6486AD","2D345D","D9BE7F","5A3A26","C6A490"}},
    {"ReneMagritte (5)" , new List<string>{"B60614","3A282F","909018","E3BFA1","EE833E"}},
    {"ReneMagritte_1 (5)" , new List<string>{"B6B3BB","697D8E","B8B87E","6F5F4B","292A2D"}},
    {"Masaccio (5)" , new List<string>{"0e2523","324028","c26b61","5a788d","de7944"}},
    {"Michelangelo (5)" , new List<string>{"42819F","86AA7D","CBB396","555234","4D280F"}},
    {"JoanMiro (5)" , new List<string>{"C04759","3B6C73","383431","F1D87F","EDE5D2"}},
    {"AmedeoModigliani (5)" , new List<string>{"1d2025","45312a","7e2f28","202938","d58e40"}},
    {"PietMondrian (5)" , new List<string>{"314290","4A71C0","F1F2ED","F0D32D","AB3A2C"}},
    {"ClaudeMonet (5)" , new List<string>{"184430","548150","DEB738","734321","852419"}},
    {"ClaudeMonet_1 (5)" , new List<string>{"9F4640","4885A4","395A92","7EA860","B985BA"}},
    {"ClaudeMonet_2 (5)" , new List<string>{"82A4BC","4C7899","2F5136","B1B94C","E5DCBE"}},
    {"EdvardMunch (5)" , new List<string>{"5059A1","EFC337","1F386E","D1AE82","BE3B2C"}},
    {"EdvardMunch_1 (5)" , new List<string>{"272A2A","E69253","EDB931","E4502E","4378A0"}},
    {"BarnettNewman (5)" , new List<string>{"442327","C0BC98","A6885D","8A3230","973B2B"}},
    {"KennethNoland (5)" , new List<string>{"D0D8CD","586180","E2AC29","1A1915","E6E1CE"}},
    {"GeorgiaOKeeffe (5)" , new List<string>{"0E122D","182044","51628E","91A1BA","AFD0C9"}},
    {"ClaesOldenburg (5)" , new List<string>{"95B1C9","263656","698946","F8D440","C82720"}},
    {"PabloPicasso (5)" , new List<string>{"CD6C74","566C7D","DD9D91","A1544B","D5898D"}},
    {"PabloPicasso_1 (5)" , new List<string>{"4E7989","A9011B","E4A826","80944E","DCD6B2"}},
    {"JacksonPollock (5)" , new List<string>{"D89CA9","1962A0","F1ECD7","E8C051","1A1C23"}},
    {"Prince (5)" , new List<string>{"735bcc","6650b4","59449c","4b3984","3e2d6c"}},
    {"JohnQuidor (5)" , new List<string>{"B79A59","826C37","54442F","DBCEAF","C4AA52"}},
    {"MelRamos (5)" , new List<string>{"C13E43","376EA5","565654","F9D502","E7CA6B"}},
    {"OdilonRedon (5)" , new List<string>{"695B8F","B26C61","C2AF46","4D5E30","8B1F1D"}},
    {"Rembrandt (5)" , new List<string>{"DBC99A","A68329","5B5224","8A350C","090A04"}},
    {"Pierre-AugusteRenoir (5)" , new List<string>{"2B5275","A69F55","F1D395","FFFBDD","D16647"}},
    {"Pierre-AugusteRenoir_1 (5)" , new List<string>{"303241","B7A067","C8C2B2","686D4F","4D3930"}},
    {"BridgetRiley (5)" , new List<string>{"FAB9AC","7BBC53","DE6736","67C1EC","E6B90D"}},
    {"JamesRosenquist (5)" , new List<string>{"E25D75","3F4C8C","6A79B0","D7BC1F","DCCFAB"}},
    {"MarkRothko (5)" , new List<string>{"E49A16","E19713","D67629","DA6E2E","D85434"}},
    {"MarkRothko_1 (5)" , new List<string>{"D5D6D1","BEC0BF","5B382C","39352C","1B1B1B"}},
    {"JohnSingerSargent (5)" , new List<string>{"b43a35","3e501e","f8f2f4","6b381d","20242d"}},
    {"JohnSingerSargent_1 (5)" , new List<string>{"778BD0","E2D76B","95BF78","4E6A3D","5F4F38"}},
    {"JohnSingerSargent_2 (5)" , new List<string>{"EEC7A0","EAA69C","BD7C96","A1A481","D97669"}},
    {"OskarSchlemmer (5)" , new List<string>{"3A488A","8785B2","DABD61","D95F30","BE3428"}},
    {"GeorgesSeurat (5)" , new List<string>{"3F3F63","808EB7","465946","8C9355","925E48"}},
    {"SandySkoglund (5)" , new List<string>{"d7f96e","457d24","879387","e1c39f","394835"}},
    {"PavelTchelitchew (5)" , new List<string>{"ac2527","f8cc5a","5c8447","61221a","2b4868"}},
    {"J M W Turner (5)" , new List<string>{"F1ECCE","9EA3B5","E9D688","A85835","AE8045"}},
    {"CyTwombly (5)" , new List<string>{"F2788F","F591EA","F0C333","F5C2AF","F23B3F"}},
    {"JohannJacobUlrich (5)" , new List<string>{"FDDDAB","E7A974","A87250","7B533D","6A4531"}},
    {"TheovanDoesburg (5)" , new List<string>{"BD748F","3D578E","BFAB68","DAD7D0","272928"}},
    {"TheovanDoesburg_1 (5)" , new List<string>{"53628D","B8B45B","C1C3B6","984F48","2E3432"}},
    {"JanvanEyck (5)" , new List<string>{"3C490C","3B5B71","262121","7C6C4E","6C2B23"}},
    {"VincentvanGogh (5)"  , new List<string>{"1a3431","2b41a7","6283c8","ccc776","c7ad24"}},
    {"VincentvanGogh_1 (5)" , new List<string>{"FBDC30","A7A651","E0BA7A","9BA7B0","5A5F80"}},
    {"VincentvanGogh_2 (5)" , new List<string>{"374D8D","93A0CB","82A866","C4B743","A35029"}},
    {"RemediosVaro (5)" , new List<string>{"C8DAAD","989E53","738D60","DEBC31","9D471A"}},
    {"DiegoVelazquez (5)" , new List<string>{"413A2C","241F1A","C5B49B","A57F5B","5C351E"}},
    {"JohannesVermeer (5)" , new List<string>{"0C0B10","707DA6","CCAD9D","B08E4A","863B34"}},
    {"JohannesVermeer_1 (5)" , new List<string>{"022F69","D6C17A","D8D0BE","6B724B","7C3E2F"}},
    {"AndyWarhol (5)" , new List<string>{"F26386","F588AF","A4D984","FCBC52","FD814E"}},
    {"AndyWarhol_1 (5)" , new List<string>{"FD0C81","FFED4D","C34582","EBA49E","272324"}},
    {"AndyWarhol_2 (5)" , new List<string>{"D32934","2F191B","2BAA92","D12E6C","F4BCB9"}},
    {"AndyWarhol_3 (5)" , new List<string>{"a99364","da95aa","f4f0e4","b74954","c2ddb2"}},
    {"GrantWood (5)" , new List<string>{"A6BDB0","8B842F","41240B","9C4823","D6AA7E"}},
    {"FrancescoXanto (5)" , new List<string>{"2C6AA5","D9AE2C","DDC655","D88C27","64894D"}},
    {"JackYoungerman (5)" , new List<string>{"59A55D","EFDB56","7D9DC6","ECA23F","CA4D2A"}},
    {"KarlZerbe (5)" , new List<string>{"46734F","CAAB6C","D0CCAF","617F97","9A352D"}},
    // Based on screenshot of picture of t-shirt
    {"Eleanor Shellstrop T-Shirt (5)" , new List<string>{"d93b2e","e8d19b","ead967","8dc6c1","31b1bc"}},
    // From https://www.weasyl.com/~colorchallenge/submissions/649404/july-color-scheme-challenge
    {"Tropical Skittles (5)" , new List<string>{"e0545e","f47b2a","fad506","a1cb3f","1692c7"}},
    // From http://www.schemecolor.com/lots-of-skittles-candy.php
    {"Skittles (5)" , new List<string>{"C51A20", "EB731F", "F5D31D", "47262F", "64BE1E"}},
    // From https://i5.walmartimages.com/asr/a5a9ba73-0596-463e-8189-70068bc2dcfe_1.e188d40fb27fbf3f8cc78a95281ac314.jpeg
    {"Wild Berry Skittles (5)" , new List<string>{"502553","2f8b39","d35d75","d11d3a","1e3970"}},
    // From https://www.color-hex.com/color-palette/40866
    {"Melancholy Rainbow (5)" , new List<string>{"2e1b1b","5f664d","ebe2b8","a26565","11143b"}},
    // from https://www.pinterest.com/pin/419608890287849018/
    {"Autumn Melancholy (5)" , new List<string>{ "d0c589","ca8441","e26815","404116","359e7f"}},
    // I created based on images from the RFQ
{"GFIAA (5)", new List<string>{"dadedd","3a4a39","7c782f","6f899e","604c35"}},
});

        // -----------------------
        // 6
        colorMap.Add(new Dictionary<string, List<string>> {
    {"Grayscale (6)" , new List<string>{"FFFFFF","CCCCCC", "999999","666666", "333333","000000"}},
    // From https://www.schemecolor.com/70s-retro.php
    {"70s Retro (6)" , new List<string>{"3F8A8C", "0C5679", "0B0835", "E5340B", "F28A0F", "FFE7BD"}},
    // From https://www.schemecolor.com/vintage-lining.php
    {"Vintage Lining (6)" , new List<string>{"498059", "C7BE81", "80574E", "AC6F62", "DAC190", "749E7E"}},
    // From https://www.schemecolor.com/old-map.php
    {"Old Map (6)" , new List<string>{"3B727C", "B9A37E", "D1BE9D", "82A775", "B05F66", "64513B"}},
    // From https://www.schemecolor.com/vintage-pixels.php
    {"Vintage Pixels (6)" , new List<string>{"3C445A", "239997", "D4AD6A", "B76B66", "C0BA89", "8A9628"}},
    // From https://www.schemecolor.com/vintage-blue-red-and-green.php
    {"Vintage Blue, Red, Green" , new List<string>{"8AB8A2", "FEE886", "DE8E4E", "CC3A3D", "306437", "4F7B58"}},
    // From https://www.schemecolor.com/classic-vintage-rainbow.php
    {"Classic Vintage Rainbow (6)" , new List<string>{"D45979", "E8A05D", "F8DC93", "4DA378", "4B5798", "A25FAD"}},
    // From https://www.schemecolor.com/vintage-truth-2.php
    {"Vintage Truth (6)" , new List<string>{"3F3B33", "92504C", "DA9F5F", "E6C89D", "748A5E", "32415E"}},
    // From https://www.schemecolor.com/vintage-hues.php
    {"Vintage Hues (6)" , new List<string>{"E05B4B", "E09336", "DAB552", "DBD9A6", "87B49C", "4B8A7E"}},
    // Next several from https://juiceboxinteractive.com/blog/color/
    {"2010s (6)" , new List<string>{"efb6bf", "fe3c71", "c86e4c", "3bd566", "dddddd", "0081fe"}},
    {"2000s (6)" , new List<string>{"df19c1", "23d513", "ff6200", "4399de", "efc40e", "e33056"}},
    {"1990s (6)" , new List<string>{"b13a1a", "287e9e", "832c76", "e4a834", "164db0", "b3346c"}},
    {"1980s (6)" , new List<string>{"3968cb", "ff68a8", "ca7cd8", "f9eb0f", "10e7e2", "ff2153"}},
    {"1970s (6)" , new List<string>{"00a1d3", "769f52", "a92da3", "ff68a8", "fd4d2e", "f8ca38"}},
    {"1960s (6)" , new List<string>{"cf4917", "985914", "f9ac3d", "d0b285", "758c33", "2d758c"}},
    {"1950s (6)" , new List<string>{"ff91bb", "f5855b", "ffd95c", "68bbb8", "4ac6d7", "e81b23"}},
    {"1940s (6)" , new List<string>{"2a326d", "4e9fbc", "f4cf0d", "dabd8f", "d90707", "b38069"}},
    {"1930s (6)" , new List<string>{"d02d1c", "99b1c9", "eed023", "345d98", "b0dabe", "3a7359"}},
    {"1920s (6)" , new List<string>{"f4b308", "ca2222", "ab933c", "34399d", "e5ce9c", "a5aaa0"}},
    // From https://www.schemecolor.com/vintage-neon.php
    {"Vintage Neon (6)" , new List<string>{"B35864", "D08A78", "D9CD97", "6A877F","C0AC77","648C49"}},
    // From https://www.schemecolor.com/vintage-vantage.php
    {"Vintage Vantage (6)" , new List<string>{"788663", "E7C899","FAEBC9", "D49166", "8B7567", "8BAFAF"}},
    // From https://www.schemecolor.com/turmeric-and-spices.php
    {"Tumeric and Spices (6)" , new List<string>{"F0C828", "A2775A", "664B41","7D8053", "7D0C0C","211F1F"}},
    // From https://colorswall.com/palette/23407
    {"Soft ui neomorphism flat (6)" , new List<string>{"da8a33","edc485","04583e","944c64","d1b6af","3a585c"}},
    // I came up with this one.
    {"Earthy (6)" , new List<string>{"8B4513","800000","DEB887","556B2F","FFD700","D2691E"}},
   
    // From https://colorswall.com/palette/12495
    {"Mental Health (6)" , new List<string>{"20c1bf","a7cae3","057672","6a8189","ece9d9","2e3e61"}},
    // From https://colorswall.com/palette/40134 (I removed 2 colors)
    {"CSS FF 6 (6)" , new List<string>{"cc4b37", "ffae00", "3adb76", "767676", "1779ba", "e6e6e6"}},
    // Next 3 from  https://loading.io/color/feature/
    {"Western Digital (6)" , new List<string>{"005195","028948","ffd400","0067b3","9d0a0e","003369"}},
    {"Zapier (6)" , new List<string>{"ff4a00","fd7622","ffc43e","5f6c72","499df3","13d0ab"}},
    {"The Scream (6)" , new List<string>{"514134","e35839","d28d4f","dbae1d","477187","323a3f"}},
    {"Hope College (6)" , new List<string>{"F46A1F","002244","F7E654","BED600","00685B","00B0CA"}},
    // From https://colorswall.com/palette/172
    {"M and M s (6)" , new List<string>{"b11224","fff200","2f9fd7","31ac55","f26f22","603a34"}},
    // Based on screenshot of picture of t-shirt
    {"Eleanor Shellstrop T-Shirt (6)", new List<string>{"d93b2e", "e8d19b", "ead967", "8dc6c1", "31b1bc","FFFFFF"}},
// I created based on images from the RFQ
{"GFIAA (6)", new List<string>{"dadedd","3a4a39","7c782f","6f899e","604c35","967e6d"}},
});
        // ---------------------------------------------------------------------------------------
        // 7
        colorMap.Add(new Dictionary<string, List<string>> {
    {"Grayscale (7)" , new List<string>{"FFFFFF", "D5D5D5", "AAAAAA", "808080", "555555", "2A2A2A", "000000"}},
    // From ? (I don"t remember where this came from, but it isn"t structly linear so it should look a bit different.
    {"Grayscale V2 (7)" , new List<string>{"FFFFFF","D4D4D4", "B4B4B4", "909090", "636363", "494848","000000"}},
    // From https://colorswall.com/palette/102
    {"Rainbow (7)" , new List<string>{"ff0000","ffa500","ffff00","008000","0000ff","4b0082","ee82ee"}},
    // From https://colorswall.com/palette/320
    {"Wing CSS (7)" , new List<string>{"111111", "a7a7a7", "f5f5f5", "0062ff", "ff1500", "ffbf00", "00b30f"}},
    // From https://colorswall.com/palette/3
    {"Bootstrap 4 (7)" , new List<string>{"0275d8", "5cb85c", "5bc0de", "f0ad4e", "d9534f", "292b2c", "f7f7f7"}},
    // From https://colorswall.com/palette/319
    {"Gumby CSS (7)" , new List<string>{"3085d6", "42a35a", "f2f2f2", "4a4d50", "ca3838", "f6b83f", "58c026"}},
    // Next 6 from https://loading.io/color/feature/
    {"LiveStream (7)" , new List<string>{"cf202e","232121","f78822","f6db35","6dc067","4185be","8f499c"}},
    {"New Balance (7)" , new List<string>{"ce2724","f3ec19","207c88","aac1bf","e8e9d7","4c4d4f","231f20"}},
    {"Olark (7)" , new List<string>{"8d989a","744da8","1fb3e0","49c219","f4dc2a","eeb417","d65129"}},
    {"Tivo (7)" , new List<string>{"da3d34","00a480","ed9f40","6a76ac","17170e","534b38","a6a480"}},
    {"Chinese Water Color (7)" , new List<string>{"832f0e","0c0a08","594a40","8e7967","e3c2a0","deaa6e","81947a"}},
    {"Starry Night (7)" , new List<string>{"151e2f","2a3e83","dad69f","feeb54","a3945d","6d82b1","303f63"}},
    {"Hope College (7)" , new List<string>{"F46A1F","002244","91420E","F7E654","BED600","00685B","00B0CA"}},
// I created based on images from the RFQ
{"GFIAA (7)", new List<string>{"dadedd","3a4a39","7c782f","6f899e","604c35","967e6d","bca885"}},
});

        // ---------------------------------------------------------------------------------------
        // 8
        colorMap.Add(new Dictionary<string, List<string>> {
    {"Grayscale (8)" , new List<string>{"FFFFFF", "dbdbdb", "b7b7b7", "929292", "6e6e6e", "494949", "252525", "000000"}},
    // From https://colorswall.com/palette/1255
    {"Glitch (8)" , new List<string>{"020202","3e3c41","fdfefe","cc0f39","0ffbf9","c2bfcc","c36b93","cdc764"}},
    // From https://colorswall.com/palette/40134
    {"CSS FF 6 (8)" , new List<string>{"cc4b37", "ffae00", "3adb76", "767676", "1779ba", "e6e6e6", "cacaca", "8a8a8a"}},
    // From https://colorswall.com/palette/2387
    { "Primer CSS (8)" , new List<string> { "6a737d", "0366d6", "28a745", "6f42c1", "ffd33d", "f66a0a", "d73a49", "ffffff" }},
    // Next 3  https://loading.io/color/feature/
    { "Last Pass (8)" , new List<string> { "d32d27", "af0809", "000000", "0c2340", "00a3e0", "ede04b", "333f48", "d9e1e2" }},
    { "McDonalds (8)" , new List<string> { "bf0c0c", "e76a05", "ffc600", "47bc00", "05007b", "9748a8", "2bb3f3", "865200" }},
    { "Texas AM (8)" , new List<string> { "500000", "003c71", "5b6236", "744f28", "998542", "332c2c", "707373", "d6d3c4" }},
    // From https://connect.pantone.com/#/trending
    { "Pantone Balancing Act (8)" , new List<string> { "c5aeb1", "e2c1c0", "d29380", "ccb97e", "6667ab", "86a293", "884c5e", "9d848e" }},
    { "Pantone Wellspring (8)" , new List<string> { "4b9b69", "75a14f", "476a30", "cec153", "b28330", "8b5897", "6667ab", "a3ccc9" }},
    { "Pantone Amusements (8)" , new List<string> { "d3806f", "6667ab", "b18f6a", "d3507a", "df88b7", "e4455e", "edc373", "86a1a9" }},
    { "Hope College (8)" , new List<string> { "F46A1F", "002244", "91420E", "F7E654", "BED600", "00685B", "5482AB", "00B0CA" }},
    { "Make A Wish (8)" , new List<string> { "0057B8", "FF585D", "FFB549", "FBD872", "00BAB3", "8BC8E8", "75787B", "BBBCBC" }},
    // from AMA Coding Art on Instagram (with permission, values converted to hex using chatGPT!)
    { "AMA Coding Art (8)" , new List<string> { "E9D8A6", "EE9B00", "CA6702", "BB3E03", "9B2226", "94D2BD", "0A9396", "005F73" }},
// I created based on images from the RFQ
{ "GFIAA (8)", new List<string> { "3a4a38", "7c782f", "6f899e", "604c35", "967e6d", "bca885", "9bb6ce", "4c9cd4" }},
    { "GFIAA 2 (8)", new List<string> { "e4e4e0", "3a4a38", "7c782f", "6f899e", "604c35", "967e6d", "bca885", "9bb6ce" }},
});
        colorMap.Add(new Dictionary<string, List<string>> {
    {"Rainbow and BW (9)" , new List<string>{"ff0000", "FFA500", "ffff00","00ff00","0000ff","4B0082","EE82EE","000000","ffffff"}},
    // From
    {"Apple (9)" , new List<string>{"62bb47","fcb827","f6821f","e03a3c","963d97","009ddc","ffffff","999999","000000"}},
    // Next 10 from https://www.omnisci.com/blog/12-color-palettes-for-telling-better-stories-with-your-data
    {"Retro Metro (9)" , new List<string>{"ea5545","f46a9b","ef9b20","edbf33","ede15b","bdcf32","87bc45","27aeef","b33dc6"}},
    {"Dutch Field (9)" , new List<string>{"e60049","0bb4ff","50e991","e6d800","9b19f5","ffa300","dc0ab4","b3d4ff","00bfa0"}},
    {"River Nights (9)", new List<string>{"b30000","7c1158","4421af","1a53ff","0d88e6","00b7c7","5ad45a","8be04e","ebdc78"}},
    {"Spring Pastels (9)" , new List<string>{"fd7f6f","7eb0d5","b2e061","bd7ebe","ffb55a","ffee65","beb9db","fdcce5","8bd3c7"}},
    {"Blue to Yellow (9)" , new List<string>{"115f9a","1984c5","22a7f0","48b5c4","76c68f","a6d75b","c9e52f","d0ee11","d0f400"}},
    {"Grey to Red (9)" , new List<string>{"d7e1ee","cbd6e4","bfcbdb","b3bfd1","a4a2a8","df8879","c86558","b04238","991f17"}},
    {"Blue to Red (9)" , new List<string>{"1984c5","22a7f0","63bff0","a7d5ed","e2e2e2","e1a692","de6e56","e14b31","c23728"}},
    {"Orange to Purple (9)" , new List<string>{"ffb400","d2980d","a57c1b","786028","363445","48446e","5e569b","776bcd","9080ff"}},
    {"Pink Foam (9)" , new List<string>{"54bebe","76c8c8","98d1d1","badbdb","dedad2","e4bcad","df979e","d7658b","c80064"}},
    {"Salmon to Aqua (9)" , new List<string>{"e27c7c","a86464","6d4b4b","503f3f","333333","3c4e4b","466964","599e94","6cd4c5"}},
    // Next 6 from https://loading.io/color/feature/
    {"Creative Commons (9)" , new List<string>{"b62b6e","9628c6","4374b7","abb8af","98c807","b1a24a","edd812","ef9421","d13814"}},
    {"Knight Foundation (9)" , new List<string>{"000000","03cce6","29c876","ff4081","fdce2e","f5f4f2","cccccc","666666","ff3939"}},
    {"LinkedIn (9)" , new List<string>{"0077b5","000000","313335","86888a","caccce","00a0dc","8d6cab","dd5143","e68523"}},
    {"Llyods (9)" , new List<string>{"d81f2a","ff9900","e0d86e","9ea900","6ec9e0","007ea3","9e4770","631d76","1e1e1e"}},
    {"Walmart (9)" , new List<string>{"007dc6","79b9e7","f47421","76c143","ffc120","e7f0f7","f2f8fd","222222","444444"}},
    {"Windows (9)" , new List<string>{"0078d7","002050","ffb900","d83b01","e81123","b4009e","5c2d91","008272","107c10"}},
    {"Hope College (9)" , new List<string>{"F46A1F","002244","91420E","F0AB00","F7E654","BED600","00685B","5482AB","00B0CA"}},
    // From image take from https://commons.wikimedia.org/wiki/File:Albers_color_model.png
    // (Also attributed to Johann Wolfgang von Goethe)
    {"Albers Color Model (9)" , new List<string>{"E32321","FFD300","2A72AF","F17B11","874B68","95A358","BC633D","C38F35","8E7760"}},
// I created based on images from the RFQ
{"GFIAA (9)", new List<string>{"e4e4e0","3a4a38","7c782f","6f899e","604c35","967e6d","bca885","9bb6ce","4c9cd4"}},
});
        colorMap.Add(new Dictionary<string, List<string>> {
    // From https://loading.io/color/feature/Spectral-10/
    {"Spectral (10)" , new List<string>{"9e0142", "d53e4f", "f46d43", "fdae61", "fee08b", "e6f598", "abdda4", "66c2a5", "3288bd", "5e4fa2"}},
    // Next 7 from  https://loading.io/color/feature/
    {"Trello (10)" , new List<string>{"0079bf","70b500","ff9f1a","eb5a46","f2d600","c377e0","ff78cb","00c2e0","51e898","c4c9cc"}},
    {"Univ of Dayton (10)" , new List<string>{"ce1141","004b8d","000000","faf2f3","d1cdb8","cf0a2c","0082ca","199051","e76829","ffdd00"}},
    {"Vodafone (10)" , new List<string>{"e60000","4a4d4e","9c2aa0","5e2750","00b0ca","007c92","a8b400","fecb00","eb9800","000000"}},
    {"Wake Forest (10)" , new List<string>{"9e7e38","000000","82231c","511536","443e67","375669","456525","59786c","3d3c1d","625750"}},
    {"WOSM (10)" , new List<string>{"622599","0054a0","aaba0a","dd7500","e23d28","3d8e33","3399ff","ff3399","fcd116","999999"}},
    {"Slack (10)" , new List<string>{"bd4030","e0b83e","7f9626","76be9f","9bcfde","599f8c","36173b","563e58","cc4876","f4f3f1"}},
    {"Emerald City (10)" , new List<string>{"985d4e","e0dcb8","aca730","4f563b","9db189","60a363","2c8c14","0a4308","607c83","466277"}},
    {"Hope College (10)" , new List<string>{"F46A1F","002244","91420E","F0AB00","F7E654","BED600","00685B","5482AB","00B0CA","BBE7E6"}}
});
        colorMap.Add(new Dictionary<string, List<string>> {
    // Next 2 from https://loading.io/color/feature/
    {"Algolia (11)" , new List<string>{"050f2c", "003666", "00aeff", "3369e7", "8e43e7", "b84592", "ff4f81", "ff6c5f", "ffc168", "2dde98", "1cc7d0"}},
    {"Continental Ag (11)" , new List<string>{"ffa500","00a5dc","004eaf","2db928","057855","ff2d37","000000","737373","969696","cdcdcd","f0f0f0"}},
    {"Hope College (11)" , new List<string>{"F46A1F","002244","91420E","F0AB00","F7E654","BED600","00685B","5482AB","00B0CA","BBE7E6","4D4F53"}}
});
        colorMap.Add(new Dictionary<string, List<string>> {
    {"Hope College (12)" , new List<string>{"F46A1F","002244","91420E","F0AB00","F7E654","BED600","00685B","5482AB","00B0CA","BBE7E6","000000","4D4F53"}}
});
        colorMap.Add(new Dictionary<string, List<string>> {
    {"Hope College (13)" , new List<string>{"F46A1F","002244","91420E","F0AB00","F7E654","BED600","00685B","5482AB","00B0CA","BBE7E6","000000","4D4F53","FFFFFF"}}
});

        //Load maps
        for (int i = 0; i <= MAX_COLORS - MIN_COLORS; i++) {
            colorMap[i].AddRange(cbColorMap[i]); // add cb palettes of this size
            colorMapAll.AddRange(colorMap[i]); // add palettes to the overall map
            colorList.Add(new List<KeyValuePair<string, List<string>>>(colorMap[i]));
            colorListAll.AddRange(colorList[i]);
            cbColorMapAll.AddRange(cbColorMap[i]);
            cbColorList.Add(new List<KeyValuePair<string, List<string>>>(cbColorMap[i]));
            cbColorListAll.AddRange(cbColorList[i]);
        }
    }

    //Check this method
    public static KeyValuePair<string, List<string>> RandomHopePalette() {
        if (random.Next(0, 2) == 0)
            return RandomPaletteLike("Hope");
        else {
            KeyValuePair<string, List<string>> p;
            do {
                p = RandomPaletteLike("Hope");
            } while (p.Value.Count < 5);
            return p;
        }
    }

    public static KeyValuePair<string, List<string>> RandomPaletteLike(string substring) {
        KeyValuePair<string, List<string>> p;
        do {
            p = RandomPalette();
        } while (!p.Value.Contains(substring));
        return p;
    }

    public static KeyValuePair<string, List<string>> RandomGrayscalePalette() {
        KeyValuePair<string, List<string>> p;
        do {
            p = RandomPalette();
        } while (!p.Value.Contains("Gray"));
        return p;
    }

    public static KeyValuePair<string, List<string>> RandomPalette(int colors = 0, int minColors = 0, bool colorBlind = false) {
        KeyValuePair<string, List<string>> palette = new KeyValuePair<string, List<string>>();
        List<KeyValuePair<string, List<string>>> currColors;
        if (colorBlind)
            currColors = cbColorListAll;
        else
            currColors = colorListAll;

        do {
            palette = currColors[random.Next(currColors.Count)];
        } while (palette.Value.Count < MIN_COLORS || palette.Value.Count > MAX_COLORS);

        return palette;
    }

    public static KeyValuePair<string, List<string>> RandomOddPalette(int colors = 0, int minColors = 0, bool colorBlind = false) {
        KeyValuePair<string, List<string>> palette;
        do {
            palette = RandomPalette(colors, minColors, colorBlind);
        }
        while (palette.Value.Count % 2 == 0);

        return palette;
    }
    //Should be string list pair so we can have name when we get a random palette
    /*public static KeyValuePair<string, List<string>> RandomPalette(int colors = 0, int minColors = 0, bool colorBlind = false) {
        List<(string, List<string>)> palette;
        if (minColors == 0) {
            if (colorBlind) {
                do {
                    palette = cbColorListAll[random.Next(cbColorListAll.Count)];
                } while (palette.Item2.Count < minColors);
            }
            else {
                do {
                    palette = colorListAll[random.Next(colorListAll.Count)];
                } while (palette.Item2.Count < minColors);
            }
        }
        else {
            if (colorBlind) {
                if (colors < MIN_COLORS || colors > MAX_COLORS)
                    palette = cb_color_list_all[random.Next(cb_color_list_all.Count)];
                else
                    palette = cb_color_list[colors][random.Next(cb_color_list[colors].Count)];
            }
            else {
                if (colors < MIN_COLORS || colors > MAX_COLORS)
                    palette = colorListAll[random.Next(colorListAll.Count)];
                else
                    palette = color_list[colors][random.Next(color_list[colors].Count)];
            }
        }
        return palette.Item2;
    }*/

    public static List<string> GetPalette(string name) {
        return colorMapAll[name];
    }

    public static (int, int, int) Blend((int, int, int) color1, (int, int, int) color2, int steps, int step) {
        int r = color1.Item1;
        int g = color1.Item2;
        int b = color1.Item3;
        int r2 = color2.Item1;
        int g2 = color2.Item2;
        int b2 = color2.Item3;
        int rr = (int)(r * (steps - 1 - step) / (steps - 1) + r2 * step / (steps - 1));
        int gg = (int)(g * (steps - 1 - step) / (steps - 1) + g2 * step / (steps - 1));
        int bb = (int)(b * (steps - 1 - step) / (steps - 1) + b2 * step / (steps - 1));
        return (rr, gg, bb);
    }

    public static (int, int, int) Darker((int, int, int) color, double percent = 0.05) {
        int r = color.Item1;
        int g = color.Item2;
        int b = color.Item3;
        r = (int)(r * (1 - percent));
        g = (int)(g * (1 - percent));
        b = (int)(b * (1 - percent));
        return (r, g, b);
    }

    public static (int, int, int) Lighter((int, int, int) color, double percent = 0.05) {
        int r = color.Item1;
        int g = color.Item2;
        int b = color.Item3;
        r = (int)(r + percent * (255 - r));
        g = (int)(g + percent * (255 - g));
        b = (int)(b + percent * (255 - b));
        return (r, g, b);
    }

    public static int ModerateOne(int rgb, double avg, double desiredAverage) {
        double len = 1 - 2 * avg;
        return (int)Math.Round((rgb * (1 - 2 * desiredAverage) + 255 * (desiredAverage - avg)) / len);
    }

    public static (int, int, int) ModerateColor((int, int, int) c, double howFarFromHalf = 0.2) {
        double r = c.Item1;
        double g = c.Item2;
        double b = c.Item3;
        double avg = (r + g + b) / (3 * 256);
        double min = 0.5 - Math.Abs(howFarFromHalf);
        double max = 0.5 + Math.Abs(howFarFromHalf);
        double da = Math.Clamp(avg, min, max);
        if (da != avg) {
            r = ModerateOne((int)r, avg, da);
            g = ModerateOne((int)g, avg, da);
            b = ModerateOne((int)b, avg, da);
        }
        return ((int)r, (int)g, (int)b);
    }

    public static List<(int, int, int)> ModeratePalette(List<string> pal) {
        List<(int, int, int)> palette = new List<(int, int, int)>();
        foreach (string c in pal) {
            var (r, g, b) = ModerateColor((int.Parse(c.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                int.Parse(c.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                int.Parse(c.Substring(4, 2), System.Globalization.NumberStyles.HexNumber)));
            palette.Add((r, g, b));
        }
        return palette;
    }

    public static List<(int, int, int)> ExpandPalette(List<(int, int, int)> palette, int numColors) {
        List<(int, int, int)> newColors = new List<(int, int, int)>();
        foreach (var c in palette) {
            int r = c.Item1;
            int g = c.Item2;
            int b = c.Item3;
            newColors.Add((r, g, b));
        }
        HashSet<(int, int, int)> colors = new HashSet<(int, int, int)>(newColors);
        while (colors.Count < numColors) {
            List<(int, int, int)> tempColors = new List<(int, int, int)>(colors);
            foreach (var c in tempColors) {
                int r = c.Item1;
                int g = c.Item2;
                int b = c.Item3;
                newColors.Add((r / 2, g / 2, b / 2));
                newColors.Add((r + (255 - r) / 2, g + (255 - g) / 2, b + (255 - b) / 2));
            }
            colors = new HashSet<(int, int, int)>(newColors);
        }
        List<(int, int, int)> clist = new List<(int, int, int)>(palette);
        clist.AddRange(colors);
        int i = 0;
        while (clist.Count < numColors) {
            clist.Add(clist[i]);
            i = (i + 1) % clist.Count;
        }
        return clist;
    }
}