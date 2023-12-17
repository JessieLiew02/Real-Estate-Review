﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PropertyWebsite.WebPages.Client
{
    public partial class Property : System.Web.UI.Page
    {
        string keyword;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                keyword = Request.QueryString["searchText"] ?? "";

                if (!string.IsNullOrEmpty(keyword))
                {
                    // Use a parameterized query to avoid SQL injection
                    SqlDataSource1.SelectCommand = "SELECT * FROM [Property] WHERE propName LIKE @keyword";
                    SqlDataSource1.SelectParameters.Clear();
                    SqlDataSource1.SelectParameters.Add("keyword", "%" + keyword + "%");
                    ViewState["searchText"] = keyword;
                }
                else
                {
                    // Reset the command to fetch all records if no search text is provided
                    SqlDataSource1.SelectCommand = "SELECT * FROM [Property]";
                }

            }
        }

        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "detail")
            {
                Response.Redirect(HttpContext.Current.Request.ApplicationPath + "WebPages/Client/PropertyDetail.aspx?Pid=" + e.CommandArgument.ToString());
            }
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            filter();

        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            filter();
        }

        protected void ddlPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            filter();
        }

        private void filter()
        {
            if (ViewState["searchText"] != null)
            {
                // Use a parameterized query to avoid SQL injection
                SqlDataSource1.SelectCommand = "SELECT * FROM [Property] WHERE propName LIKE @keyword";
                SqlDataSource1.SelectParameters.Clear();
                SqlDataSource1.SelectParameters.Add("keyword", "%" + ViewState["searchText"].ToString() + "%");
            }
            else
            {
                // Reset the command to fetch all records if no search text is provided
                SqlDataSource1.SelectCommand = "SELECT * FROM [Property] WHERE area LIKE @area AND category LIKE @category";
                SqlDataSource1.SelectParameters.Clear();
            }

            if (ddlCategory.SelectedValue != "*")
            {
                SqlDataSource1.SelectCommand += " AND category = @category";
                SqlDataSource1.SelectParameters.Add("category", ddlCategory.SelectedValue);

            }
            if (ddlArea.SelectedValue != "*")
            {
                SqlDataSource1.SelectCommand += " AND area = @area";
                SqlDataSource1.SelectParameters.Add("area", ddlArea.SelectedValue);

            }

            if (ddlPrice.SelectedValue == "ASC")
            {
                SqlDataSource1.SelectCommand += " ORDER BY startPrice ASC";
            }
            else
            {
                SqlDataSource1.SelectCommand += " ORDER BY endPrice DESC";
            }
        }
    }
}