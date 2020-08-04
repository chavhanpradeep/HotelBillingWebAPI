using System;
using DAL.Models;
using System.IO;
using System.Text;

namespace Utilities
{
    public class TemplateGenerator
    {
        private StringBuilder _stringbuilder;

        public string GetHTMLString(Bill bill)
        {
            _stringbuilder = new StringBuilder();

            var sb = new StringBuilder();
            _stringbuilder.AppendFormat(@"
                <html>
                    <head>
                        <title>
                            Demo
                        </title>
                    </head>
                    <body>
                        <table align='center'>
                            <tr>
                                <td colspan='4' text-align='center'>
                                Sales Receipt
                                </td>
                            </tr>
                            <tr>
                                <td colspan='4'>
                                    Date: {0}
                                </td>
                            </tr>
                            <tr>
                                <th>Description</th>
                                <th>Unit Price</th>
                                <th>Quantity</th>
                                <th>Price</th>
                            </tr>", bill.CreatedOn.Date.ToString("dd MMM yyyy"));

            foreach (var billDetail in bill.BillDetails)
            {
                _stringbuilder.AppendFormat(@"<tr>
                        <td>{0}</td>
                        <td>{1}</td>
                        <td>{2}</td>
                        <td>{3}</td>
                </tr>", billDetail.ItemName, billDetail.Price, billDetail.Quantity, (billDetail.Price * billDetail.Quantity));
            }
            
            _stringbuilder.AppendFormat(@"
                <tr>
                    <td colspan='3' align='right'>
                    Sub Total
                    </td>
                    <td>{0}</td>
                </tr>", String.Format("{0:0.00}", bill.SubTotal));

            _stringbuilder.AppendFormat(@"
                <tr>
                    <td colspan='3' align='right'>Discount ({0}%)</td>
                    <td>{1}</td>
                </tr>", bill.DiscountPercentage, String.Format("{0:0.00}", bill.DiscountAmount));

            _stringbuilder.AppendFormat(@"
                <tr>
                    <td colspan='3' align='right'>VAT (5%)</td>
                    <td>{0}</td>
                </tr>", String.Format("{0:0.00}", bill.VATAmount));

            _stringbuilder.AppendFormat(@"
                <tr>
                    <td colspan='3' align='right'>Total</td>
                    <td>{0}</td>
                </tr>", String.Format("{0:0.00}", bill.TotalPrice));

            _stringbuilder.Append(@"
                </table>
                </body>
                </html>
            ");

            return _stringbuilder.ToString();
        }
    }
}
