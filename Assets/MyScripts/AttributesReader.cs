using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;


public class AttributesReader : MonoBehaviour
{

    public List<Pipe> Pipes;
    public string Filepath;
    public TextAsset attr_file;
    public GameObject point_prefab;
    public bool initialised;
    public enum Type_e { Main, Secondary, Tertiary };
    public enum Diameter_e
     
    {
        Φ100, Φ120, Φ125, Φ130, Φ150, Φ155, Φ175, Φ200, Φ50, Φ75, Φ80, Φ85, Φ90
    };
    public enum Material_e { AsbestosCementPipe, DuctileIronPipe, UPVCpipe };

    public class Pipe
    {
        private int Id;
        private Type_e Type;
        private Diameter_e Diameter;
        private Material_e Material;
        private decimal Length_Geo; /* We use decimals because we need precision in numbers , since they are GPS */
        private decimal Start_X;
        private decimal Start_Y;
        private decimal Mid_X;
        private decimal Mid_Y;
        private decimal End_X;
        private decimal End_Y;
        private decimal Inside_X;
        private decimal Inside_Y;

        

        public Pipe(int iD, Type_e type, Diameter_e diameter, Material_e material, decimal length_Geo, decimal start_X,
            decimal start_Y, decimal mid_X, decimal mid_Y, decimal end_X, decimal end_Y, decimal inside_X, decimal inside_Y)
        {
            Id = iD;
            Type = type;
            Diameter = diameter;
            Material = material;
            Length_Geo = length_Geo;
            Start_X = start_X;
            Start_Y = start_Y;
            Mid_X = mid_X;
            Mid_Y = mid_Y;
            End_X = end_X;
            End_Y = end_Y;
            Inside_X = inside_X;
            Inside_Y = inside_Y;
        }

        public Pipe()
        {
        }

        public int getId()
        {
            return this.Id;
        }

        public void setId(int Id)
        {
            this.Id = Id;
        }

        public Type_e getType()
        {
            return this.Type;
        }

        public void setType(Type_e Type)
        {
            this.Type = Type;
        }

        public Diameter_e getDiameter()
        {
            return this.Diameter;
        }

        public void setDiameter(Diameter_e Diameter)
        {
            this.Diameter = Diameter;
        }

        public Material_e getMaterial()
        {
            return this.Material;
        }

        public void setMaterial(Material_e Material)
        {
            this.Material = Material;
        }

        public decimal getLength_Geo()
        {
            return this.Length_Geo;
        }

        public void setLength_Geo(decimal Length_Geo)
        {
            this.Length_Geo = Length_Geo;
        }

        public decimal getStart_X()
        {
            return this.Start_X;
        }

        public void setStart_X(decimal Start_X)
        {
            this.Start_X = Start_X;
        }

        public decimal getStart_Y()
        {
            return this.Start_Y;
        }

        public void setStart_Y(decimal Start_Y)
        {
            this.Start_Y = Start_Y;
        }

        public decimal getMid_X()
        {
            return this.Mid_X;
        }

        public void setMid_X(decimal Mid_X)
        {
            this.Mid_X = Mid_X;
        }

        public decimal getMid_Y()
        {
            return this.Mid_Y;
        }

        public void setMid_Y(decimal Mid_Y)
        {
            this.Mid_Y = Mid_Y;
        }

        public decimal getEnd_X()
        {
            return this.End_X;
        }

        public void setEnd_X(decimal End_X)
        {
            this.End_X = End_X;
        }

        public decimal getEnd_Y()
        {
            return this.End_Y;
        }

        public void setEnd_Y(decimal End_Y)
        {
            this.End_Y = End_Y;
        }

        public decimal getInside_X()
        {
            return this.Inside_X;
        }

        public void setInside_X(decimal Inside_X)
        {
            this.Inside_X = Inside_X;
        }

        public decimal getInside_Y()
        {
            return this.Inside_Y;
        }

        public void setInside_Y(decimal Inside_Y)
        {
            this.Inside_Y = Inside_Y;
        }

        public void toString()
        {
            Debug.Log("{Id : " + this.getId().ToString()  + "} {Type : "+ this.getType().ToString() + "} {Diameter : " + this.getDiameter().ToString()
                + "} {Material : " + this.getMaterial().ToString() + "} {Length Geo : " + this.getLength_Geo() + "} {Start X : " +
                this.getStart_X() + "} {Start Y : "+ this.getStart_Y() + "} {Mid X :" + this.getMid_X() + "} {Mid Y : " + this.getMid_Y()
                + "} {End X :" + this.getEnd_X() + "} {End Y : " + this.getEnd_Y() + "} {Inside X : " + this.getInside_X() + 
                "} {Inside Y : " + this.getInside_Y()
                );
        }





    }

    public  List<Pipe> LoadAttributesFile(TextAsset path){

        List<Pipe> pipes = new List<Pipe>();

        int counter = 0;
        //string line;


        // Read the file and display it line by line.  
        /*System.IO.StreamReader file =
        new System.IO.StreamReader(path);*/
        string[] linesInFile = path.text.Split('\n');
        /*while ((line = path.ReadLine()) != null)
        {*/
        foreach (string line in linesInFile)
        {
            Pipe pipe = new Pipe();
            //Debug.Log(line);
            //first line is Headers
            if (counter > 0)
            {
                string[] words = line.Split(';');
                int j = 0;
                //Debug.Log("Length is : " + words.Length);

                /*
                 *Each line contains 13 attributes for each pipe.
                 *We use delimiter to seperate ; between atributes and then we
                 *store each one in Pipe . We avoid 1st string of each line because
                 *Id's are listed like : 0:1 , 1:2 , 2:3 and we need only the second one.
                 *Example :
                 * this is the first line that we ignore , thats why we use counter > 0 above
                 * FID;ID;Type;Diameter;Material;LENGTH_GEO;START_X;START_Y;MID_X;MID_Y;END_X;END_Y;INSIDE_X;INSIDE_Y 
                   0;1;Tertiary;Φ80;UPVC Pipe;2,254261640280000;25,132336907599999;35,333133560699999;25,132333435500001;35,333123808000003;25,132329963400000;35,333114055400003;25,132333435500001;35,333123808000003
                 * */
                foreach (var word in words)
                {
                    if (j > 0)
                    {
                        //Debug.Log(word);
                        switch (j)
                        {
                            case 1:
                                pipe.setId(int.Parse(word));
                                break;
                            case 2:
                                switch (word)
                                {
                                    case "Main":
                                        pipe.setType(Type_e.Main);
                                        break;
                                    case "Secondary":
                                        pipe.setType(Type_e.Secondary);
                                        break;
                                    case "Tertiary":
                                        pipe.setType(Type_e.Tertiary);
                                        break;
                                }
                                break;
                            case 3:
                                switch (word)
                                {
                                    case "Φ100":
                                        pipe.setDiameter(Diameter_e.Φ100);
                                        break;
                                    case "Φ120":
                                        pipe.setDiameter(Diameter_e.Φ120);
                                        break;
                                    case "Φ125":
                                        pipe.setDiameter(Diameter_e.Φ125);
                                        break;
                                    case "Φ130":
                                        pipe.setDiameter(Diameter_e.Φ130);
                                        break;
                                    case "Φ150":
                                        pipe.setDiameter(Diameter_e.Φ150);
                                        break;
                                    case "Φ155":
                                        pipe.setDiameter(Diameter_e.Φ155);
                                        break;
                                    case "Φ175":
                                        pipe.setDiameter(Diameter_e.Φ175);
                                        break;
                                    case "Φ200":
                                        pipe.setDiameter(Diameter_e.Φ200);
                                        break;
                                    case "Φ50":
                                        pipe.setDiameter(Diameter_e.Φ50);
                                        break;
                                    case "Φ75":
                                        pipe.setDiameter(Diameter_e.Φ75);
                                        break;
                                    case "Φ80":
                                        pipe.setDiameter(Diameter_e.Φ80);
                                        break;
                                    case "Φ85":
                                        pipe.setDiameter(Diameter_e.Φ85);
                                        break;
                                    case "Φ90":
                                        pipe.setDiameter(Diameter_e.Φ90);
                                        break;

                                }
                                break;
                            case 4:
                                switch (word)
                                {
                                    case "Asbestos Cement pipe":
                                        pipe.setMaterial(Material_e.AsbestosCementPipe);
                                        break;
                                    case "Ductile iron pipe":
                                        pipe.setMaterial(Material_e.DuctileIronPipe);
                                        break;
                                    case "UPVC Pipe":
                                        pipe.setMaterial(Material_e.UPVCpipe);
                                        break;
                                }
                                break;
                            case 5: /* We need to use Separator because by default numbers use ',' instead of '.' in floating point */
                                pipe.setLength_Geo(decimal.Parse(word, new NumberFormatInfo { NumberDecimalSeparator = "," }));
                                break;
                            case 6:
                                pipe.setStart_X(decimal.Parse(word, new NumberFormatInfo { NumberDecimalSeparator = "," }) );
                                break;
                            case 7:
                                pipe.setStart_Y(decimal.Parse(word, new NumberFormatInfo { NumberDecimalSeparator = "," }));
                                break;
                            case 8:
                                pipe.setMid_X(decimal.Parse(word,new NumberFormatInfo { NumberDecimalSeparator = "," }));
                                break;
                            case 9:
                                pipe.setMid_Y(decimal.Parse(word ,new NumberFormatInfo { NumberDecimalSeparator = "," }));
                                break;
                            case 10:
                                pipe.setEnd_X(decimal.Parse(word, new NumberFormatInfo { NumberDecimalSeparator = "," }));
                                break;
                            case 11:
                                pipe.setEnd_Y(decimal.Parse(word, new NumberFormatInfo { NumberDecimalSeparator = "," }));
                                break;
                            case 12:
                                pipe.setInside_X(decimal.Parse(word, new NumberFormatInfo { NumberDecimalSeparator = "," }));
                                break;
                            case 13:
                                pipe.setInside_Y(decimal.Parse(word, new NumberFormatInfo { NumberDecimalSeparator = "," }));
                                break;

                            default:
                                    Debug.Log("Wrong input , out of bounds");
                                    break;
                        }
                    
                }
                    j++;
                }

                /*Testing if initialisation went well */
                pipes.Add(pipe);
                //pipe.toString();
            }
           
            counter++;
        }

        //file.Close();


       
        return pipes;
    }

    
    



    // Start is called before the first frame update
    void Start()
    {

        

        initialised = false;
        Pipes = LoadAttributesFile(attr_file);
        initialised = true;

    }

    // Update is called once per frame
    void Update()
    {

    }

}
