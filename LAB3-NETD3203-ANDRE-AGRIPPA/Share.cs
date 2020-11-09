/*
 * Name: Andre Agrippa
 * Date: 11/05/2020
 * Course: NETD 3202
 * Purpose: Share base class
 * File: Share.cs
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace LAB3_NETD3203_ANDRE_AGRIPPA
{
    public class Share
    { 
        //Data members
        protected string buyerName;
        protected string buyDate;
        protected int numberOfShares;
        protected string shareType;

        //Default constructor
        public Share(string buyerName, string buyDate, int numberOfShares, string shareType)
        {
            this.buyerName = buyerName;
            this.buyDate = buyDate;
            this.numberOfShares = numberOfShares;
            this.shareType = shareType;
        }
        
        //Getter and setter for BuyerName
        public string BuyerName 
        {
            get { return this.buyerName; }
            set { this.buyerName = value; }
        }
        //Getter and Setter for BuyDate
        public string BuyDate
        {
            get { return this.buyDate; }
            set { this.buyDate = value; }
        }
        //Getter and Setter for NumberOfShares
        public int NumberOfShares
        {
            get { return this.numberOfShares; }
            set { this.numberOfShares = value; }
        }

        //Getter and Setter for ShareType
        public string ShareType
        {
            get { return this.shareType; }
            set { this.shareType = value; }
        }
    }
}
