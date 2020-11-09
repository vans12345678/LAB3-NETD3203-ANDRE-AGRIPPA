/*
 * Name: Andre Agrippa
 * Date: 11/05/2020
 * Course: NETD 3202
 * Purpose: PreferredShare subclass
 * File: PreferredShare.cs
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace LAB3_NETD3203_ANDRE_AGRIPPA
{
    public class PreferredShare : Share
    {
        //Default constructor for PreferredShare
        public PreferredShare(int sharePrice, int shareVotingPower, string buyerName, string buyDate, int numberOfShares, string shareType) : base(buyerName, buyDate, numberOfShares, shareType)
        {
            this.buyerName = base.buyerName;
            this.buyDate = base.buyDate;
            this.sharePrice = sharePrice;
            this.shareVotingPower = shareVotingPower;
            this.numberOfShares = numberOfShares;
            this.shareType = shareType;
        }

        //Data members
        private int sharePrice;
        private int shareVotingPower;

        //Getter and setter for SharePrice
        public int SharePrice
        {
            get { return this.sharePrice; }
            set { this.sharePrice = value; }
        }
        //Getter and Setter for ShareVotingPower
        public int ShareVotingPower
        {
            get { return this.shareVotingPower; }
            set { this.shareVotingPower = value; }
        }
    }
}
