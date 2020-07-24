//-----------------------------------------------------------------------
// <copyright company="EMC Corporation">
//
// Copyright © 2012-2013 EMC Corporation.  All rights reserved.
//
// </copyright>
//-----------------------------------------------------------------------

namespace Custom.InputAccel.UimScript
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using Emc.InputAccel.CaptureClient;
    using Emc.InputAccel.CaptureFlow;
    using Emc.InputAccel.UimScript;
    using System.Windows.Forms;
    using System.Collections;
    using Newtonsoft.Json;
    using System.IO;
    using System.Data;
    using System.Json;
    using System.Net;
    using System.Xml;
    using System.Numerics;
    using System.Data.SqlClient;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Windows.Documents;
    using System.Threading;


    /// <summary>
    /// The base class for script assemblies.
    /// </summary>
    public sealed class ScriptMain : UimScriptMainCore
    {
        public ArrayList prelist;
        public ArrayList prelistTable;
        public ArrayList prelistDropDown;
        public static DataTable dt { get; set; }
        public IUimFieldDataContext hiddenfield;
        public string servernameSOAP { get; set; }


        public string id { get; set; }
        public ScriptMain()
                : base()
        { }

        protected override void ModuleBatchListView(string moduleType, List<ITableRow> tableRowList, string loginName, string[] departments)
        {
            //MessageBox.Show("test doc");
        }


        public void hiddenSection(IUimDataEntryFormContext form, string hiddenCommons)
        {
            IUimFormSectionContext segmentSection;
            segmentSection = form.FindFormSection("COMMONS");
            if (hiddenCommons == "Y")
            {
                segmentSection.Show(false);

            }
            else if (hiddenCommons == "N")
            {
                segmentSection.Show(true);

            }
            else
            {
                segmentSection.Show(false);

            }


        }


        //---------------------------------- Json Checkbox and Table ---------------------------------//
        public ArrayList getLabelField(IUimDataEntryFormContext form)
        {
            prelist = new ArrayList();
            IUimDataContext dataContext = form.UimDataContext;
            IUimFormControlContext[] formcontext = form.GetControlContextArray();
            foreach (IUimFormControlContext fc in formcontext)
            {
                if (fc.ControlName.StartsWith("lbl_"))
                {
                    //string[] grouplist = fc.ControlName.Split('_');
                    //prelist.Add(grouplist[1] + "_" + grouplist[2]);
                    //MessageBox.Show(grouplist[1] + "_" + grouplist[2]);
                    string grouplist = fc.ControlName.Substring(4, fc.ControlName.Length - 4);
                    //MessageBox.Show(grouplist);
                    prelist.Add(grouplist);
                }

            }
            return prelist;
        }

        public ArrayList getLabelTable(IUimDataEntryFormContext form)
        {
            prelistTable = new ArrayList();
            string grouplist = string.Empty;

            IUimFormControlContext[] formcontext = form.GetControlContextArray();
            //MessageBox.Show("getLabelTable");
            foreach (IUimFormControlContext fc in formcontext)
            {

                if (fc.FormSection.FormSectionName.StartsWith("tbl_"))
                {
                    if (grouplist != fc.FormSection.FormSectionName)
                    {
                        grouplist = fc.FormSection.FormSectionName;
                        //MessageBox.Show(grouplist);
                        prelistTable.Add(grouplist);
                    }
                    else { continue; }

                }


            }

            return prelistTable;
        }
        public ArrayList getLabelDropDown(IUimDataEntryFormContext form)
        {
            prelistDropDown = new ArrayList();
            string grouplist = string.Empty;

            IUimFormControlContext[] formcontext = form.GetControlContextArray();
            System.Windows.MessageBox.Show("getLabelDropDown");
            foreach (IUimFormControlContext fc in formcontext)
            {
                //System.Windows.MessageBox.Show(fc.ControlName);
                if (fc.ControlName.StartsWith("ddl_"))
                {
                    if (grouplist != fc.FormSection.FormSectionName)
                    {
                        grouplist = fc.FormSection.FormSectionName;
                       
                        prelistDropDown.Add(grouplist);
                    }
                    else { continue; }

                }


            }
            System.Windows.MessageBox.Show("DROPDOWNLIST COUNT"+prelistDropDown.Count.ToString());
            return prelistDropDown;
        }

        public void jsonFormatTable(IUimDataContext dataContext, ArrayList prelistTable)
        {
            ArrayList listRow = new ArrayList();
            foreach (object table in prelistTable)
            {
                //MessageBox.Show("START FOR LOOP TABLE");

                IUimTableSectionContext tablecontext = dataContext.FindTableSection(table.ToString());
                // MessageBox.Show("TABLE NAME:" +table.ToString());
                // MessageBox.Show("Count Row:" +tablecontext.RowCount.ToString());
                string[] column = tablecontext.GetFieldNames();
                foreach (string c in column)
                {
                    //MessageBox.Show("START FOR COLUMN");
                    //MessageBox.Show("TABLE:"+table.ToString()+ "COLUMN :"+c);
                    for (int i = 0; i < tablecontext.RowCount; i++)
                    {
                        IUimFieldDataContext datafield = tablecontext.GetFieldAt(i, c);
                        string value = datafield.ValueAsString;
                        listRow.Add(value);

                    }

                    string jsonformat = JsonConvert.SerializeObject(listRow);
                    listRow.Clear();
                    string hname = c.Substring(2);
                    hiddenfield = dataContext.FindFieldDataContext(hname);
                    hiddenfield.SetValue(jsonformat);
                    //MessageBox.Show("TABLE NAME =" +table.ToString());
                    //MessageBox.Show("Set Field Hidden =" + hname);
                    //MessageBox.Show("VALUE =" +jsonformat);

                }


            }

        }

        public void jsonFormatDropDown(IUimDataContext dataContext, ArrayList prelistDropDown)
        {
            ArrayList list= new ArrayList();
            foreach (object v in prelistDropDown)
            {

                  

            }


            
        }

        public void jsonFormat(IUimDataContext dataContext, ArrayList prelist)
        {
            ArrayList list = new ArrayList();
            IUimFieldDataContext[] checkbox = dataContext.GetFieldDataContextArray();

            foreach (IUimFieldDataContext cb in checkbox)
            {
                foreach (object s in prelist)
                {
                    if (cb.Name.StartsWith(s.ToString()))
                    {
                        list.Add(cb.Name);
                    }
                }
            }

            ArrayList list2 = new ArrayList();
            string fieldname = null;
            foreach (object fname in list)
            {
                IUimFieldDataContext field;
                string[] value = fname.ToString().Split(new string[] { "_S_", "_M_" }, StringSplitOptions.None);
                string select_value = value[1];
                field = dataContext.FindFieldDataContext(fname.ToString());
                string hname = value[0];
                hiddenfield = dataContext.FindFieldDataContext(hname);

                if (fieldname != hname)
                {
                    list2.Clear();
                }

                if (field.Value.ToString() == "1")
                {
                    if (hname == "bankTransferSelected")
                    {
                        list2.Add(select_value);
                        string jsonresult = JsonConvert.SerializeObject(list2);
                        hiddenfield.SetValue(jsonresult);
                        fieldname = hname;
                    }
                    else
                    {
                        list2.Add(select_value);
                        string jsonresult = JsonConvert.SerializeObject(list2);
                        hiddenfield.SetValue(jsonresult);
                        fieldname = hname;
                    }
                }
                else
                {
                    if (hname == "bankTransferSelected")
                    {
                        list2.Add("N");
                        string jsonresult = JsonConvert.SerializeObject(list2);
                        hiddenfield.SetValue(jsonresult);
                        fieldname = hname;
                    }
                }
            }


        }
       

        //------------------------------------------ Data Object Class.------------------------------------//

        public class LookupDataID
        {
            // lookupID = ???
            public string lookupID { get; set; }

        }
        public class DataObject 
        {
            public string text { get; set; }
            public string value { get; set; }
        }
        public class DataObjectFundName
        {
            //FUNDNAME
            // public string FundCode { get; set; }
            public string ShortDesc { get; set; }
            public string DescriptionTh { get; set; }
            // public string DescriptionEn { get; set; }

            // public string ShortDescTH { get; set; }

        }
        public class LookupDropDownsBankName
        {
            // DROPDOWN BANKNAME
            public string BankName { get; set; }
            public string BankCode { get; set; }
     
        }
        public class LookupDropDownsProvince
        {
            //DROPDOWN PROVINCE
            public string countryCode { get; set; }
            public string provinceCode { get; set; }
            public string provinceName { get; set; }
            public string lookupID { get; set; }

        }
        public class LookupDropDownsZipcode
        {
            // DROPDOWN ZIPCODE (POSTCODE)
            public string districtName { get; set; }
            public string provinceCode { get; set; }
            public string postCode { get; set; }
            public string lookupID { get; set; }

        }
  
        public class LookupPolicy
        {
            // POLICY
            public string TitleName { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string idCardNo { get; set; }

            public string policyNo { get; set; }
            public string lookupID { get; set; }
            public string birthDate { get; set; }
            //"TitleName": "ด.ช.                                    ",
            //"firstName": "คงศักดิ์",
            //"lastName": "เพ็ญเกษกร",
            //"idCardNo": "1101400653948",
            //"policyNo": "A00000045"
        }
        public class LookupIDCard
        {
            // ID

            public string identityNo { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string birthDate { get; set; }
            public string lookupID { get; set; }

  
        }

        //--------------------------------------------------------------Call REST API For DATA. -------------------------------------//
        public void restFundName(IUimDataContext datacontext)
        {
            DataTable table = new DataTable();
            List<DataObjectFundName> list = new List<DataObjectFundName>();
            DataSet dts = new DataSet();
            string URL = selectCustomValue(datacontext.CustomValue, "RestURL=");
            //string URL = "https://api.fwd.co.th/dev-mock/dataentryFacadeService/fetchFundMasterData";
            // string urlParameters = "?api_key=123";
            try
            {
                LookupDataID lookup = new LookupDataID() { lookupID = "FUNDMASTER" };
                string json = JsonConvert.SerializeObject(lookup);
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.
               
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
               
                //HttpResponseMessage response = client.GetAsync(string.Empty).Result;  //GET REST API

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL);
                request.Content = new StringContent(json);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(string.Empty, request.Content).Result;
               
                //System.Windows.MessageBox.Show("REST API");
                if (response.IsSuccessStatusCode)
                {
                    //System.Windows.MessageBox.Show(response.IsSuccessStatusCode.ToString());
                   
                    // Parse the response body.
                    IEnumerable<DataObjectFundName> dataObjects = response.Content.ReadAsAsync<IEnumerable<DataObjectFundName>>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                   
                   // System.Windows.MessageBox.Show("START LOOP");
                    foreach (var d in dataObjects)
                    {
                        //System.Windows.MessageBox.Show("LOOP 1 ");
                        string sdt = d.ShortDesc + "-" + d.DescriptionTh;
                        list.Add(new DataObjectFundName
                        {

                            ShortDesc = d.ShortDesc,
                            DescriptionTh = d.DescriptionTh + "[" + d.ShortDesc + "]"

                        });
                    }

                   // table = ToDataTable(list);
                    dt = ToDataTable(list);
                }
                else
                {
                    System.Windows.MessageBox.Show("API FAIL");
                }
                //MessageBox.Show("START-5");
                client.Dispose();
                //return dt;
            }
            catch (Exception e)
            {

                System.Windows.MessageBox.Show("Rest API Fund Name Fail !"+e.ToString());
            }
            //return table;

        }

        // Dropdownlist
        public List<LookupDropDownsBankName> restDropDownBankName(IUimDataContext datacontext , string lookupIDquery) 
        {
            List<LookupDropDownsBankName> items = new List<LookupDropDownsBankName>();
            string URL = selectCustomValue(datacontext.CustomValue, "RestURL=");
            //System.Windows.MessageBox.Show("URL REST = " + URL);
            string urlParameter = string.Empty; //Request Here
            try
            {
                LookupDataID lookup = new LookupDataID() { lookupID = lookupIDquery };
                string json = JsonConvert.SerializeObject(lookup);
               // System.Windows.MessageBox.Show(json);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // List data response.
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL);
                request.Content = new StringContent(json);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(string.Empty, request.Content).Result;
               
                if (response.IsSuccessStatusCode)
                {
                   
                    // Parse the response body.
                    IEnumerable<LookupDropDownsBankName> lookupBankName = response.Content.ReadAsAsync<IEnumerable<LookupDropDownsBankName>>().Result;
                    //Make sure to add a reference to System.Net.Http.Formatting.dll
                    foreach (var d in lookupBankName)
                    {
                        // Edit Value or Text here
                        items.Add(new LookupDropDownsBankName { BankName = d.BankName, BankCode = d.BankCode });
                    }
   
                }
                else
                {
                    System.Windows.MessageBox.Show("API Dropdownlist :"+lookupIDquery +" Fail !");
                }
                client.Dispose();
                return items;
            }
            catch (Exception e)
            {

                System.Windows.MessageBox.Show(e.ToString());
            }
            return items;


        }
        public List<LookupDropDownsProvince> restDropDownProvince(IUimDataContext datacontext, string lookupIDquery)
        {
            List<LookupDropDownsProvince> items = new List<LookupDropDownsProvince>();
            string URL = selectCustomValue(datacontext.CustomValue, "RestURL=");
            //System.Windows.MessageBox.Show("URL REST = " + URL);
            string urlParameter = string.Empty; //Request Here
            try
            {
                LookupDataID lookup = new LookupDataID() { lookupID = lookupIDquery };
                string json = JsonConvert.SerializeObject(lookup);
                // System.Windows.MessageBox.Show(json);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // List data response.
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL);
                request.Content = new StringContent(json);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(string.Empty, request.Content).Result;

                if (response.IsSuccessStatusCode)
                {

                    // Parse the response body.
                    IEnumerable<LookupDropDownsProvince> lookupBankName = response.Content.ReadAsAsync<IEnumerable<LookupDropDownsProvince>>().Result;
                    //Make sure to add a reference to System.Net.Http.Formatting.dll
                    foreach (var d in lookupBankName)
                    {
                        // Edit Value or Text here
                        items.Add(new LookupDropDownsProvince { provinceName = d.provinceName, provinceCode = d.provinceCode });
                    }

                }
                else
                {
                    System.Windows.MessageBox.Show("API Dropdownlist :" + lookupIDquery + " Fail !");
                }
                client.Dispose();
                return items;
            }
            catch (Exception e)
            {

                System.Windows.MessageBox.Show(e.ToString());
            }
            return items;


        }

        public List<LookupDropDownsZipcode> restDropDownZipcode(IUimDataContext datacontext, string lookupIDquery , string provinceCode)
        {
            List<LookupDropDownsZipcode> items = new List<LookupDropDownsZipcode>();
            string URL = selectCustomValue(datacontext.CustomValue, "RestURL=");
            //System.Windows.MessageBox.Show("URL REST = " + URL);
            string urlParameter = string.Empty; //Request Here
            try
            {
                LookupDropDownsZipcode lookup = new LookupDropDownsZipcode() { lookupID = lookupIDquery , provinceCode = provinceCode };
                string json = JsonConvert.SerializeObject(lookup);
                // System.Windows.MessageBox.Show(json);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // List data response.
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL);
                request.Content = new StringContent(json);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(string.Empty, request.Content).Result;

                if (response.IsSuccessStatusCode)
                {

                    // Parse the response body.
                    IEnumerable<LookupDropDownsZipcode> lookupZipcode = response.Content.ReadAsAsync<IEnumerable<LookupDropDownsZipcode>>().Result;
                    //Make sure to add a reference to System.Net.Http.Formatting.dll
                    foreach (var d in lookupZipcode)
                    {
                        // Edit Value or Text here
                        items.Add(new LookupDropDownsZipcode { districtName = d.districtName, postCode = d.postCode });
                    }

                }
                else
                {
                    System.Windows.MessageBox.Show("API Dropdownlist :" + lookupIDquery + " Fail !");
                }
                client.Dispose();
                return items;
            }
            catch (Exception e)
            {

                System.Windows.MessageBox.Show(e.ToString());
            }
            return items;


        }

        public List<LookupPolicy> restPolicy(IUimDataContext datacontext , string lookupIDquery , string policyNoSearch)
        {
  
            List<LookupPolicy> list = new List<LookupPolicy>();
            string URL = selectCustomValue(datacontext.CustomValue, "RestURL=");
            //string URL = "https://api.fwd.co.th/dev-mock/dataentryFacadeService/fetchFundMasterData";
            // string urlParameters = "?api_key=123";
            try
            {
                LookupPolicy lookup = new LookupPolicy() { lookupID = lookupIDquery , policyNo = policyNoSearch };
                string json = JsonConvert.SerializeObject(lookup);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL);
                request.Content = new StringContent(json);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(string.Empty, request.Content).Result;

                //System.Windows.MessageBox.Show("REST API");
                if (response.IsSuccessStatusCode)
                {
                    //System.Windows.MessageBox.Show(response.IsSuccessStatusCode.ToString());

                    // Parse the response body.
                    IEnumerable<LookupPolicy> lookupPolicy = response.Content.ReadAsAsync<IEnumerable<LookupPolicy>>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll

                    // System.Windows.MessageBox.Show("START LOOP");
                    foreach (var d in lookupPolicy)
                    {
                     
                        list.Add(new LookupPolicy { TitleName = d.TitleName , firstName = d.firstName , lastName = d.lastName, idCardNo = d.idCardNo , policyNo = d.policyNo , birthDate = d.birthDate });
                    }
                  

                }
                else
                {
                    System.Windows.MessageBox.Show("API Lookup Policy FAIL");
                }



                client.Dispose();
                return list;
            }
            catch (Exception e)
            {

                System.Windows.MessageBox.Show(e.ToString());
            }
            return list;

        }
        public List<LookupIDCard> restIDCard(IUimDataContext datacontext, string lookupIDquery, string idCardNoSearch)
        {

            List<LookupIDCard> list = new List<LookupIDCard>();
            string URL = selectCustomValue(datacontext.CustomValue, "RestURL=");
            //string URL = "https://api.fwd.co.th/dev-mock/dataentryFacadeService/fetchFundMasterData";
            // string urlParameters = "?api_key=123";
            try
            {
                LookupIDCard lookup = new LookupIDCard() { lookupID = lookupIDquery, identityNo = idCardNoSearch };
                string json = JsonConvert.SerializeObject(lookup);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL);
                request.Content = new StringContent(json);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(string.Empty, request.Content).Result;

                //System.Windows.MessageBox.Show("REST API");
                if (response.IsSuccessStatusCode)
                {
                    //System.Windows.MessageBox.Show(response.IsSuccessStatusCode.ToString());

                    // Parse the response body.
                    IEnumerable<LookupIDCard> lookupPolicy = response.Content.ReadAsAsync<IEnumerable<LookupIDCard>>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll

                    // System.Windows.MessageBox.Show("START LOOP");
                    foreach (var d in lookupPolicy)
                    {

                        list.Add(new LookupIDCard { firstName = d.firstName, lastName = d.lastName, birthDate = d.birthDate });
                    }


                }
                else
                {
                    System.Windows.MessageBox.Show("API Lookup ID Card FAIL");
                }



                client.Dispose();
                return list;
            }
            catch (Exception e)
            {

                System.Windows.MessageBox.Show(e.ToString());
            }
            return list;

        }

        //--------------------------------Lookup ID , Policy-----------------------------------------------//
        public void LookupRestPolicy(IUimFormControlContext controlContext, IUimDataEntryFormContext form, string fieldName)
        {


            IUimDataContext datacontext = form.UimDataContext;
            string fname = controlContext.FieldDataContext.Name;
            try
            {
                if (fname == fieldName )
                {
                    string value = controlContext.FieldDataContext.ValueAsString;
                   // System.Windows.MessageBox.Show("Value policy = "+  value);
                    if (!string.IsNullOrEmpty(value))
                        //Check Empty in Policy Field
                    {
                        var result = restPolicy(form.UimDataContext, "POLICYOWNER", value);
                        if (result.Count == 0)
                        {
                            setLookupPolicy(form, false, "", "", "","");
                            System.Windows.MessageBox.Show("Client not Found!");

                        }
                        else
                        {
                            setLookupPolicy(form, true, result[0].firstName, result[0].lastName, result[0].idCardNo, result[0].birthDate);

                        }
                    }
                 

                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
            }
            
        }
        public void LookupRestID(IUimFormControlContext controlContext, IUimDataEntryFormContext form, string fieldName)
        {


            IUimDataContext datacontext = form.UimDataContext;
            string fname = controlContext.FieldDataContext.Name;
            try
            {
                if (fname == fieldName)
                {
                    string value = controlContext.FieldDataContext.ValueAsString;
                    // System.Windows.MessageBox.Show("Value ID = "+  value);
                    BigInteger num;
                    bool flag = BigInteger.TryParse(value, out num);
                    if (!string.IsNullOrEmpty(value) && value.Length == 13 && flag)
                    //Check Empty in ID Field
                    {
                        // Edit at GETCLIENT
                        var result = restIDCard(form.UimDataContext, "GETCLIENT", value);
                        if (result.Count == 0)
                        {
                            setLookupIDCard(form, false, "", "", "");
                            System.Windows.MessageBox.Show("Client not Found!");

                        }
                        else
                        {
                            setLookupIDCard(form, true, result[0].firstName, result[0].lastName, result[0].birthDate);

                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("invalid format!");

                    }


                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
            }

        }

        //-------------------------SET VALUE----------- REST API---//
        internal void setLookupPolicy(IUimDataEntryFormContext form, bool flag, string sfirstname, string slastname, string identityNo, string sdob)
        {
            IUimFieldDataContext vname;
            if (flag)
            {
                if (form.UimDataContext.DocumentName == "idCard" || form.UimDataContext.DocumentName == "idCardClaim")
                {
                    IUimDataContext firstName = form.UimDataContext;
                    vname = firstName.FindFieldDataContext("firstName");
                    vname.SetValue(sfirstname);

                    IUimDataContext lastName = form.UimDataContext;
                    vname = lastName.FindFieldDataContext("lastName");
                    vname.SetValue(slastname);

                    IUimDataContext idcard = form.UimDataContext;
                    vname = idcard.FindFieldDataContext("identityNo");
                    vname.SetValue(identityNo);

                    IUimDataContext dob = form.UimDataContext;
                    vname = dob.FindFieldDataContext("dob");
                    vname.SetValue(sdob);
                }
                else
                {
                    IUimDataContext firstName = form.UimDataContext;
                    vname = firstName.FindFieldDataContext("firstName");
                    vname.SetValue(sfirstname);

                    IUimDataContext lastName = form.UimDataContext;
                    vname = lastName.FindFieldDataContext("lastName");
                    vname.SetValue(slastname);

                    IUimDataContext idcard = form.UimDataContext;
                    vname = idcard.FindFieldDataContext("identityNo");
                    vname.SetValue(identityNo);
                }
            }
            else
            {

                //IUimDataContext firstName = form.UimDataContext;
                //vname = firstName.FindFieldDataContext("firstName");
                //vname.SetValue("");

                //IUimDataContext lastName = form.UimDataContext;
                //vname = lastName.FindFieldDataContext("lastName");
                //vname.SetValue("");

                //IUimDataContext idcard = form.UimDataContext;
                //vname = idcard.FindFieldDataContext("identityNo");
                //vname.SetValue("");
            }

        }
        internal void setLookupIDCard(IUimDataEntryFormContext form, bool flag, string sfirstname, string slastname, string sdob)
        {
            IUimFieldDataContext vname;
            //System.Windows.MessageBox.Show("Document NAME in SET :"+form.UimDataContext.DocumentName.ToString());
            if (flag)
            {
                

                if (form.UimDataContext.DocumentName == "idCard" || form.UimDataContext.DocumentName == "idCardClaim" )
                {
                    IUimDataContext firstName = form.UimDataContext;
                    vname = firstName.FindFieldDataContext("firstName");
                    vname.SetValue(sfirstname);

                    IUimDataContext lastName = form.UimDataContext;
                    vname = lastName.FindFieldDataContext("lastName");
                    vname.SetValue(slastname);

                    //// dd/mm/yyyy Change
                    IUimDataContext dob = form.UimDataContext;
                    vname = dob.FindFieldDataContext("dob");
                    vname.SetValue(sdob);
                }
                else
                {
                    IUimDataContext firstName = form.UimDataContext;
                    vname = firstName.FindFieldDataContext("firstName");
                    vname.SetValue(sfirstname);

                    IUimDataContext lastName = form.UimDataContext;
                    vname = lastName.FindFieldDataContext("lastName");
                    vname.SetValue(slastname);
                }
        
            }
            else
            {
                //IUimDataContext firstName = form.UimDataContext;
                //vname = firstName.FindFieldDataContext("firstName");
                //vname.SetValue(sfirstname);

                //IUimDataContext lastName = form.UimDataContext;
                //vname = lastName.FindFieldDataContext("lastName");
                //vname.SetValue(slastname);

                //// dd/mm/yyyy Change
                //IUimDataContext dob = form.UimDataContext;
                //vname = dob.FindFieldDataContext("dob");
                //vname.SetValue(sdob);
            }

        }
        internal void setDropdownBankName(IUimDataEntryFormContext form, List<LookupDropDownsBankName> list, string nameDropdownlist)
        {
            try
            {
                //System.Windows.MessageBox.Show("list.Count ="+ list.Count.ToString() );
                IUimFormControlContext fcontrol = form.FindControl(nameDropdownlist);

                List<string> Name = new List<string>();
                List<string> Code = new List<string>();

                foreach (var s in list)
                {
                    Name.Add(s.BankName);
                    Code.Add(s.BankCode);
                }

               
                fcontrol.SetListItems(Name.ToArray(), Code.ToArray());

            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
            }



        }
        internal void setDropdownProvince(IUimDataEntryFormContext form, List<LookupDropDownsProvince> list, string nameDropdownlist)
        {
            try
            {
                ////System.Windows.MessageBox.Show("list.Count ="+ list.Count.ToString() );
                IUimFormControlContext fcontrol = form.FindControl(nameDropdownlist);
                //List<string> Name = new List<string>();
                //List<string> Code = new List<string>();
                //foreach (var s in list)
                //{
                //    Name.Add(s.provinceName);
                //    Code.Add(s.provinceCode);
                //}

                string[] text = list.Select(l => l.provinceName).ToList().ToArray();
                string[] value = list.Select(l => l.provinceCode).ToList().ToArray();

                fcontrol.SetListItems(text, value);

            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
            }



        }
        internal void setDropdownZipcode(IUimDataEntryFormContext form, List<LookupDropDownsZipcode> list, string nameDropdownlist)
        {
            try
            {

                IUimFormControlContext fcontrol = form.FindControl(nameDropdownlist);
                // GROUPBY Linq at here

                var result = list.GroupBy(u => u.postCode).Select(grp => grp.ToList());
                List<DataObject> listObject = new List<DataObject>();

                //Loop and Select First GroupBy for Add to list
                foreach (LookupDropDownsZipcode l in result.Select(o => o.First()))
                {
                    listObject.Add(new DataObject {text = l.districtName,value = l.postCode });
                }
                //Select and Set to Array
                string[] text = listObject.Select(o => o.text).ToArray();
                string[] value = listObject.Select(o => o.value).ToArray();

                fcontrol.SetListItems(value, value);


            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
            }



        }
        internal void setDropdownNull(IUimDataEntryFormContext form, string nameDropdownlist)
        {
            try
            {
                ////System.Windows.MessageBox.Show("list.Count ="+ list.Count.ToString() );
                IUimFormControlContext fcontrol = form.FindControl(nameDropdownlist);
                fcontrol.SetListItems(null, null);

            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
            }



        }
        //SET VALUE SOAP
        //public void LookupIDCardSoap(IUimFormControlContext controlContext, IUimDataEntryFormContext form, string fieldName)
        //{

        //    // MessageBox.Show("FIELD FOCUS :" + controlContext.FieldDataContext.Name);
        //    // Check path Soap in APISoap
        //    IUimDataContext datacontext = form.UimDataContext;
        //    //servernameSOAP = selectCustomValue(datacontext.CustomValue, "Servername=");
        //    string fname = controlContext.FieldDataContext.Name;
        //    BigInteger n = 0;
        //    int lenght_value = controlContext.FieldDataContext.ValueAsString.Length;
        //    bool flag = BigInteger.TryParse(controlContext.FieldDataContext.ValueAsString, out n);

        //    if (fname == fieldName)
        //    {


        //        servernameSOAP = selectCustomValue(datacontext.CustomValue, "SoapURL=");
        //        setValueSoap(fname, controlContext, form);

        //    }
        //}

        //internal void setValueSoap(string fieldname, IUimFormControlContext controlContext, IUimDataEntryFormContext form)
        //{
        //    // MessageBox.Show("START - SET SOAP");
        //    IUimDataContext datacontext = form.UimDataContext;
        //    IUimFieldDataContext fname = datacontext.FindFieldDataContext(fieldname);
        //    string fvalue = fname.ValueAsString;
        //    int lenght = fvalue.Length;

        //    string sfirstname = string.Empty;
        //    string slastname = string.Empty;
        //    string dob = string.Empty;
        //    BigInteger num;
        //    bool flag = BigInteger.TryParse(fvalue, out num);
        //    // if (fvalue.Length == 13 && flag)

        //    try
        //    {
        //        if (flag && fvalue.Length == 13 && !string.IsNullOrEmpty(fvalue))
        //        {
        //            APISoap.captivaPortService soap = new APISoap.captivaPortService();
        //            soap.Url = servernameSOAP;
        //            APISoap.poppulateDataRequest req = new APISoap.poppulateDataRequest() { identittyNo = fvalue };
        //            sfirstname = soap.poppulateData(req).firstName.ToString();
        //            slastname = soap.poppulateData(req).lastName.ToString();
        //            //dob = soap.poppulateData(req).


        //            if (string.IsNullOrEmpty(sfirstname) && string.IsNullOrEmpty(slastname))
        //            {
        //                setLookupIDCardSoap(form, false, "", "", "");
        //                System.Windows.MessageBox.Show("Client not Found!");

        //            }
        //            else
        //            {
        //                //ADD DOB --------> at Here ""
        //                setLookupIDCardSoap(form, true, sfirstname, slastname, "");

        //            }
        //        }

        //        else
        //        {
        //            System.Windows.MessageBox.Show("Invalid ID-Card Format!");
        //        }
        //    }

        //    catch (Exception e)
        //    {
        //        string error = e.StackTrace;
        //        setLookupIDCardSoap(form, false, "", "", "");
        //        System.Windows.MessageBox.Show("Client not Found!");

        //    }

        //}

        //internal void setLookupIDCardSoap(IUimDataEntryFormContext form, bool flag, string sfirstname, string slastname, string sdob)
        //{
        //    IUimFieldDataContext vname;
        //    try
        //    {
        //        if (flag)
        //        {

        //            IUimDataContext firstName = form.UimDataContext;
        //            vname = firstName.FindFieldDataContext("firstName");
        //            vname.SetValue(sfirstname);

        //            IUimDataContext lastName = form.UimDataContext;
        //            vname = lastName.FindFieldDataContext("lastName");
        //            vname.SetValue(slastname);

        //            //// dd/mm/yyyy Change
        //            //IUimDataContext dob = form.UimDataContext;
        //            //vname = dob.FindFieldDataContext("dob");
        //            //vname.SetValue(sdob);
        //        }
        //        else
        //        {

        //            IUimDataContext firstName = form.UimDataContext;
        //            vname = firstName.FindFieldDataContext("firstName");
        //            vname.SetValue("");

        //            IUimDataContext lastName = form.UimDataContext;
        //            vname = lastName.FindFieldDataContext("lastName");
        //            vname.SetValue("");

        //            //IUimDataContext dob = form.UimDataContext;
        //            //vname = dob.FindFieldDataContext("dob");
        //            //vname.SetValue("");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        System.Windows.MessageBox.Show(e.ToString());
        //    }



        //}
        //---------------------------------------------------///

        //---------------- Helper -----------------------------//
        internal string selectCustomValue(string customValue, string searchKey)
        {
            string result_select = string.Empty;
            try
            {

                // MessageBox.Show("CustomValue = " + customValue);
                // MessageBox.Show("searchKey = " + searchKey);
                if (string.IsNullOrEmpty(customValue))
                {
                    return string.Empty;
                }
                string[] arrayCustomValue = customValue.Split('|');
                foreach (string result in arrayCustomValue)
                {
                    // search = Servername=
                    if (result.StartsWith((searchKey)))
                    {
                        result_select = result.Substring(searchKey.Length);
                        // MessageBox.Show("Result ="+ result_select);
                        return result_select;
                    }

                }

            }
            catch (Exception e)
            {
                string error = e.StackTrace;

            }
            return result_select;

        }
        internal DataTable ToDataTable<T>(List<T> items)

        {

            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)

            {

                //Setting column names as Property names

                dataTable.Columns.Add(prop.Name);

            }

            foreach (T item in items)

            {

                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)

                {

                    //inserting property values to datatable rows

                    values[i] = Props[i].GetValue(item, null);

                }

                dataTable.Rows.Add(values);

            }

            //put a breakpoint here and check datatable

            return dataTable;

        }
        public void parseValueDropdown(IUimFormControlContext controlContext, IUimDataEntryFormContext form, string fieldName , int  step)
        {
            //Step 1  Simple value = code
            //Step 2  Province select get Text and keep value
            //Step 3  Simple value = text
            //Step 4  Empty


            IUimDataContext datacontext = form.UimDataContext;
            string fnamedropdown = controlContext.ControlName;
            // System.Windows.MessageBox.Show("Name Control " +fnamedropdown);

            try
            {
                if (fnamedropdown == fieldName)
                {
                    // System.Windows.MessageBox.Show("START PARSE VALUE DDL");
                    string value = form.FindControl(fieldName).ChoiceValue;
                    string text = form.FindControl(fieldName).Text;
                    //System.Windows.MessageBox.Show(value1);

                    if (step == 1)
                    {
                        IUimFieldDataContext fname = datacontext.FindFieldDataContext(fieldName.Substring(4));
                        fname.SetValue(value);
                    }
                    else if (step == 2)
                    {
                        IUimFieldDataContext fname = datacontext.FindFieldDataContext(fieldName.Substring(4));
                        fname.SetValue(text);
                        //System.Windows.MessageBox.Show("Set Field :" + fieldName.Substring(4) + "[VALUE]:" + text);

                        IUimFieldDataContext fname2 = datacontext.FindFieldDataContext(fieldName.Substring(4)+"Code");
                        fname2.SetValue(value);
                       // System.Windows.MessageBox.Show("Set Field :" + fieldName.Substring(4)+"Code"+ "[VALUE]:" + value);

                    }
                    else if (step == 3)
                    {
                        IUimFieldDataContext fname = datacontext.FindFieldDataContext(fieldName.Substring(4));
                        fname.SetValue(text);
                    }
                    else if (step == 4)
                    {
                        IUimFieldDataContext fname = datacontext.FindFieldDataContext(fieldName.Substring(4));
                        fname.SetValue(string.Empty);
                    }


                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
            }
        }
        public void parseValueEmpty(IUimDataEntryFormContext form, string fieldName)
        {

            IUimDataContext datacontext = form.UimDataContext;
            try
            {

                IUimFieldDataContext fname = datacontext.FindFieldDataContext(fieldName.Substring(4));
                fname.SetValue(string.Empty);

            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
            }
        }


    }
}



       