using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO; 
using System.Collections;


    public partial class loop : System.Web.UI.Page

    {   public static string subject = "This will be added later"; 
        public static string projectName2 = "This will be added later";
        public static string webmasterlink = "If you have any questions or problems, please contact the webmaster by email at <a href=mailto:support@media-query.com?subject=" + subject + ">support@media-query.com</a> and include '" + projectName2 + "' in the email so we can better serve you."; 
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CMGResearchConnectionString"].ConnectionString);
        public string LID= "1";
        public string qnum; 
        public static string items;
        static int index;
        public static string arrayIndex; 
        static int currentquestion;
        static string rq15;
        static int indexChecked;
        static string randomDTW;
        static string randomCheck;
        static string randomString;
        
     //arrays for random numbers
        static int randomNumber = 3;  //main array for the number of times page loops for png or items

        static List<string> shuffledList = new List<string>();//randomize the images 
        static List<string> numberChecked = new List<string>();//store values selected in the multipunch 
        static List<string> shuffledList2 = new List<string>();//randomize the multipunch 
        static List<string> multiRand = new List<string>();//store data for multipunch as ints
        static List<int> selectedList = new List<int>();

        static List<string> setArray = new List<string>() //declare array with index 0 as "" to avoid any zero index confusion 
     
        {    
            "",
            "Andrews Federal Credit Union",
            "Navy Federal Credit Union",
            "Pentagon Federal Credit Union"

        }; //this array is for competitive list

        static List<string> itemsArray = new List<string>() //starts at zero and is incremented later in code
     
        {    
            "Morning",
            "Midday",
            "Afternoon",  
            "Evening",
            "Late night",

        }; //this array is for multipunch list
        
        protected void Page_Load(object sender, EventArgs e)

        {
            
    
            //LID = Convert.ToString(Request.QueryString["LID"]); //for this test I used the QueryString for future we would use cookie below
            //LID = Request.Cookies["MusicResearch"]["LID"]; 
            string goback = Convert.ToString(Request.QueryString["yes"]);


            if (!IsPostBack)
            { //initilize in this 'if' so that update panel does not update entire page and refresh values 
                numberChecked.Clear(); //clear the numberChecked for the next page
                String cmdSelect = "SELECT randomstringDTW, randomstringCheck, currentquestion15 FROM Data_805 WHERE loginId = @LID;";
                SqlCommand cmd = new SqlCommand(cmdSelect, con);      
                cmd.Parameters.AddWithValue("@LID", LID);
                con.Open();
                //call reader to see if we have a value in randomstring and current question (this validates if this is reentry of the page 
                SqlDataReader dtrReader = cmd.ExecuteReader();
                while (dtrReader.Read())
                {
                    randomDTW = dtrReader["randomstringDTW"].ToString();
                    randomCheck = dtrReader["randomstringCheck"].ToString();
                    string currentq = dtrReader["currentquestion15"].ToString();
                    if (!string.IsNullOrEmpty(currentq))
                    {
                        currentquestion = Convert.ToInt32(dtrReader["currentquestion15"]);
                    }
                    else
                    {
                        currentquestion = 1; 
                    }
                }
                dtrReader.Close();
                con.Close();

                if (!string.IsNullOrEmpty(randomDTW))
                {

                    readDataTable(); 
                   
                   
                   
                }

                else
                {
                    List<string> numberList = new List<string>(); //creates a sting of numbers to randomize later
                    Random rnd = new Random();

                    for (int i = 1; i < randomNumber + 1; i++)
                    {
                        numberList.Add(string.Format("{0}", i));
                    }
                    //****************************randomize the numbers*********************************
                    shuffledList = numberList.OrderBy(i => rnd.Next()).ToList();
                    shuffledList.Insert(0, "");  //add the extra comma in front of the list
                    randomDTW = string.Join(",", shuffledList);
                    //sets values for 1st page load
                    currentquestion = 1; //sets current question for inital page load and any subsequent refreshes  
                    arrayIndex = shuffledList[currentquestion]; //get value of 1st index in the random list
                    index = Int32.Parse(arrayIndex); //parse into an array to be passed into answerarray index
                    items = setArray[index]; //initialization of 'items' so that page does not contain blanks on first page load


                    //****************************random strings ************************************
                    Random rnd2 = new Random();
                    shuffledList2 = itemsArray.OrderBy(i => rnd2.Next()).ToList();
                                 
                    //convert shuffled2 strings into ints for sql storage 
                    for (int i = 0; i < shuffledList2.Count; i++)
                        {
                   
                        string check = shuffledList2[i];
                        int listCheck = itemsArray.FindIndex(str => str.Contains(check));
                        multiRand.Add(string.Format("{0}", listCheck));
                  
                        }
                    randomCheck = string.Join(",", multiRand);  //For converting stings to int for sql               
                    text2.Text = string.Join(",", shuffledList2);

                    shuffledList2.Insert(5, "None of the above");  //add any mutally exclusive items or none random items here - None, Other so forth (index starts at 0)

                    // bind the data to the checkboxlist
                    CheckBoxList1.DataSource = shuffledList2;
                    CheckBoxList1.DataBind();

                }
            }

      }



        protected void Continue_Click(object sender, EventArgs e)
        {

            if (currentquestion <= randomNumber) //the number of times the page irritates 
            {
              
                for (int i = 0; i < CheckBoxList1.Items.Count; i++)
                {
                    if (CheckBoxList1.Items[i].Selected)
                    {

                        rq15 = CheckBoxList1.Items[i].Value;

                        indexChecked = shuffledList2.FindIndex(str => str.Contains(rq15));  //This code finds the text that was checked and gets the index for writing the data in the table so we can parse it into a list of ints 
                        if (indexChecked >= 0)
                        {
                            numberChecked.Add(string.Format("{0}", indexChecked + 1));
                            
                        }

                    }
                }
               
                string num = string.Join(",", numberChecked);
                String statement = "UPDATE Data_805 SET randomstringDTW = @randomstringDTW, randomstringCheck = @randomstringCheck, currentquestion15 = @currentq, q15_" + index + " = @q15, q15b_" + index + " =@q15b, q15av_" + index + " =@q15av WHERE loginId = @ID;"; //creates the sql query statement 
                SqlCommand cmd = new SqlCommand(statement, con); //provided to issue the statement to sql and avoid sql injection 
                cmd.Parameters.AddWithValue("@randomstringDTW", randomDTW); //adds value to the above varirables to further prevent sql injection 
                cmd.Parameters.AddWithValue("@randomstringCheck", randomCheck);
                cmd.Parameters.AddWithValue("@ID", LID);
                cmd.Parameters.AddWithValue("@q15", num);
                cmd.Parameters.AddWithValue("@q15b", q15b.SelectedValue);
                cmd.Parameters.AddWithValue("@q15av", q15av.Text);
                cmd.Parameters.AddWithValue("@currentq", currentquestion);

              
            
          
                //this for loop is to split the list string and write to seperate fields in sql
                //for (int i = 0; i < numberChecked.Count; i++)
                //{
                //    qnum = numberChecked[i];
              
                //    String qstatement = "UPDATE Data_805 SET q15_" + index + "_" + qnum + " = @checkList  WHERE loginId = @ID;"; //creates the sql query statement 
                //    SqlCommand qcmd = new SqlCommand(qstatement, con);        
                //    qcmd.Parameters.AddWithValue("@ID", LID);
                //    qcmd.Parameters.AddWithValue("@checkList", qnum);
                //    try
                //    {
                //        con.Open();
                //        qcmd.ExecuteNonQuery();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}


                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); //used for modifying data : Insert, Delete, Update 
              
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
         
                con.Close();
      
                    currentquestion += 1; //increment before array index to account for value on page load

                    if (currentquestion == randomNumber + 1)
                    {
                        Response.Redirect("redirectLoop.aspx");
                    }
                    else
                    {
                        arrayIndex = shuffledList[currentquestion];
                        index = Int32.Parse(arrayIndex);
                        items = setArray[index]; //this loops through 'items' (if there are any) 
                        numberChecked.Clear(); //clear the numberChecked for the next page

                        for (int k = 0; k < CheckBoxList1.Items.Count; k++)//this will clear out the checkbox by running through all the checkbox, only clearing the selected checkboxes by setting them false
                        {
                            if (CheckBoxList1.Items[k].Selected)
                            {
                                CheckBoxList1.Items[k].Selected = false;
                            }
                        }

                        q15b.ClearSelection();
                        q15av.Text = "";
                    }
             
            }
            readDataTable();                  
        }

        protected void Back_Click(object sender, EventArgs e)

        {
            
            if (currentquestion <= randomNumber)
            {
                if (currentquestion != 1)
                {
                    currentquestion = currentquestion - 1;
                    arrayIndex = shuffledList[currentquestion];
                    index = Int32.Parse(arrayIndex);
                    items = setArray[index]; //this loops through 'items' (if there are any) 

                    String statement = "UPDATE Data_805 SET currentquestion15 = @currentq WHERE loginId = @ID;"; //creates the sql query statement 
                    SqlCommand cmd = new SqlCommand(statement, con); //provided to issue the statement to sql and avoid sql injection 
                    cmd.Parameters.AddWithValue("@ID", LID);    
                    cmd.Parameters.AddWithValue("@currentq", currentquestion);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery(); //used for modifying data : Insert, Delete, Update 

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    con.Close();

                    readDataTable(); 

       
                }
                else
                {
                    Response.Redirect("redirectLoop.aspx"); 
                
                }
            }
        }



        protected void updateDataTable()
        {


        }

        protected void readDataTable()
        {

            shuffledList = randomDTW.Split(',').ToList();

            for (int i = 0; i < shuffledList2.Count; i++)
            {

                string check = shuffledList2[i];
                shuffledList2.Add(string.Format("{0}", check));                        
            }
  
         
            arrayIndex = shuffledList[currentquestion]; //get value of 1st index in the random list
            index = Int32.Parse(arrayIndex); //parse into an array to be passed into answerarray index


            String cmdGoback = "SELECT q15_" + index + ", q15b_" + index + ", q15av_" + index + " FROM Data_805 WHERE loginId = @LID;";
            SqlCommand cmdGb = new SqlCommand(cmdGoback, con);
            cmdGb.Parameters.AddWithValue("@LID", LID);
            con.Open();

            SqlDataReader dtrReaderGB = cmdGb.ExecuteReader();


            while (dtrReaderGB.Read())
            {

                q15av.Text = dtrReaderGB["q15av_" + index].ToString();
                q15b.SelectedValue = dtrReaderGB["q15b_" + index].ToString();
                randomString = dtrReaderGB["q15_" + index].ToString();
                if (!string.IsNullOrEmpty(randomString))
                {
                    selectedList = randomString.Split(',').Select(int.Parse).ToList();
                }

            }


            dtrReaderGB.Close();
            con.Close();

            CheckBoxList1.DataSource = shuffledList2;
            CheckBoxList1.DataBind();
            if (!string.IsNullOrEmpty(randomString))
            {
                string choice = "";
                for (int k = 0; k < selectedList.Count; k++)
                {
                    int sel = selectedList[k];
                    sel = sel - 1;
                    choice = shuffledList2[sel];

                    for (int i = 0; i < CheckBoxList1.Items.Count; i++)
                    {
                        string boxValue = CheckBoxList1.Items[i].Value;
                        if (boxValue == choice)
                        {
                            CheckBoxList1.Items[i].Selected = true;
                        }
                    }

                }

            }
        }
     
    }
