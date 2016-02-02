using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCCalculator.Controllers
{
    public class CalculatorController : Controller
    {

        private const String message = "The actual sample MVC Calculator.";
        /// <summary>
        /// define the KeyPad values
        /// I suppose generic x/y coordinates
        /// would be better throughout, since
        /// it would ease changing the 
        /// entire layout and functionality,
        /// btn11, btn 12, btn21 etc.
        /// </summary>
        private const String n0 = "0";
        private const String n1 = "1";
        private const String n2 = "2";
        private const String n3 = "3";
        private const String n4 = "4";
        private const String n5 = "5";
        private const String n6 = "6";
        private const String n7 = "7";
        private const String n8 = "8";
        private const String n9 = "9";

        private const String divide = "/";
        private const String multiply = "*";
        private const String subtract = "-";
        private const String add = "+";
        private const String rsign = "+/-";
        private const String adddec = ".";
        private const String clear = "C";
        private const String memplus = "M+";
        private const String memrecall = "MR";
        private const String calculate = "=";
        


        /// <summary>
        /// Set up "constant" values, gather form 
        /// "state" data.
        /// </summary>
        /// <returns></returns>
        private ActionResult LoadCalculator(ControllerContext context, Boolean put)
        {
            /// Set up "constants"
            this.ViewBag.Message = message;
            ViewBag.n0 = n0;
            ViewBag.n1 = n1;
            ViewBag.n2 = n2;
            ViewBag.n3 = n3;
            ViewBag.n4 = n4;
            ViewBag.n5 = n5;
            ViewBag.n6 = n6;
            ViewBag.n7 = n7;
            ViewBag.n8 = n8;
            ViewBag.n9 = n9;
            ViewBag.divide = divide;
            ViewBag.multiply = multiply;
            ViewBag.subtract = subtract;
            ViewBag.add = add;
            ViewBag.rsign = rsign;
            ViewBag.adddec = adddec;
            ViewBag.clear = clear;
            ViewBag.memplus = memplus;
            ViewBag.memrecall = memrecall;
            ViewBag.calculate = calculate;

            /// Gather "state"
            if (put)
            {
                ViewBag.displayvalue = context.HttpContext.Request.Form.Get("displayvalue");
                ViewBag.memorystored = context.HttpContext.Request.Form.Get("memorystored");
                ViewBag.runningresult = context.HttpContext.Request.Form.Get("runningresult");
                ViewBag.currentoperation = context.HttpContext.Request.Form.Get("currentoperation");
                ViewBag.prevoperation = context.HttpContext.Request.Form.Get("prevoperation");
                ViewBag.prevdisplayvalue = context.HttpContext.Request.Form.Get("prevdisplayvalue");
            }
            else
            {
                ViewBag.displayvalue = String.Empty;
                ViewBag.memorystored = String.Empty;
                ViewBag.runningresult = String.Empty;
                ViewBag.currentoperation = String.Empty;
                ViewBag.prevdisplayvalue = String.Empty;
                ViewBag.prevoperation = String.Empty;
            }

            return View();
        }
        // GET: Calculator
        public ActionResult Calculator()
        {
            return LoadCalculator(this.ControllerContext, false);
        }

        // PUT: Calculator
        public ActionResult Calculator(string submit, ControllerContext context)
        {
            ViewResult resultsView = (ViewResult)LoadCalculator(context, true);


            switch (submit)
            {
                case clear:
                    resultsView.ViewBag.displayvalue = String.Empty;
                    resultsView.ViewBag.currentoperation = String.Empty;
                    resultsView.ViewBag.prevdisplayvalue = String.Empty;
                    resultsView.ViewBag.runningresult = String.Empty;
                    resultsView.ViewBag.prevoperation = String.Empty;
                    break;
                case rsign:
                    resultsView.ViewBag.displayvalue = ViewBag.displayvalue.ToString().IndexOf("-") < 0 ? "-" + ViewBag.displayvalue : resultsView.ViewBag.displayvalue.ToString().Replace("-",String.Empty);
                    resultsView.ViewBag.prevdisplayvalue = resultsView.ViewBag.displayvalue;
                    break;
                case memplus:
                    /// this should actuall DoMath against the current memorystored...
                    resultsView.ViewBag.memorystored = DoMath(add,resultsView.ViewBag.memorystored,resultsView.ViewBag.displayvalue);
                    break;
                case memrecall:
                    resultsView.ViewBag.displayvalue = resultsView.ViewBag.memorystored;
                    break;
                case adddec:
                    resultsView.ViewBag.displayvalue = resultsView.ViewBag.displayvalue += resultsView.ViewBag.displayvalue.ToString().IndexOf(".") < 0 ? "." : String.Empty;
                    break;
                case divide:
                    resultsView.ViewBag.currentoperation = divide;
                    resultsView.ViewBag.prevoperation = divide;
                    if (!String.IsNullOrEmpty(resultsView.ViewBag.prevdisplayvalue.ToString()))
                    {
                        resultsView.ViewBag.runningresult = DoMath(resultsView.ViewBag.currentoperation, resultsView.ViewBag.runningresult, resultsView.ViewBag.prevdisplayvalue);
                    }
                    else
                    {
                        resultsView.ViewBag.runningresult = resultsView.ViewBag.displayvalue;
                    }
                    resultsView.ViewBag.prevdisplayvalue = resultsView.ViewBag.displayvalue;
                    resultsView.ViewBag.runningresult = resultsView.ViewBag.displayvalue;
                    resultsView.ViewBag.displayvalue = String.Empty;
                    break;
                case multiply:
                    resultsView.ViewBag.currentoperation = multiply;
                    resultsView.ViewBag.prevoperation = multiply;
                    if (!String.IsNullOrEmpty(resultsView.ViewBag.prevdisplayvalue.ToString()))
                    {
                        resultsView.ViewBag.runningresult = DoMath(resultsView.ViewBag.currentoperation, resultsView.ViewBag.runningresult, resultsView.ViewBag.prevdisplayvalue);
                    }
                    else
                    {
                        resultsView.ViewBag.runningresult = resultsView.ViewBag.displayvalue;
                    }
                    resultsView.ViewBag.prevdisplayvalue = resultsView.ViewBag.displayvalue;
                    resultsView.ViewBag.runningresult = resultsView.ViewBag.displayvalue;
                    resultsView.ViewBag.displayvalue = String.Empty;
                    break;
                case add:
                    resultsView.ViewBag.currentoperation = add;
                    resultsView.ViewBag.prevoperation = add;
                    if (resultsView.ViewBag.recentrunning == null || resultsView.ViewBag.recentrunning.ToString().IndexOfAny(add + subtract + divide + multiply) >= 0)
                    {
                        resultsView.ViewBag.prevdisplayvalue = resultsView.ViewBag.displayvalue;
                        resultsView.ViewBag.runningresult = DoMath(resultsView.ViewBag.currentoperation, resultsView.ViewBag.runningresult, resultsView.ViewBag.prevdisplayvalue);
                    }
                    else
                    {
                        resultsView.ViewBag.runningresult = DoMath(resultsView.ViewBag.currentoperation, resultsView.ViewBag.runningresult, resultsView.ViewBag.displayvalue);
                    }
                    resultsView.ViewBag.displayvalue = String.Empty;
                    break;
                case subtract:
                    resultsView.ViewBag.currentoperation = subtract;
                    resultsView.ViewBag.prevoperation = subtract;
                    resultsView.ViewBag.prevdisplayvalue = resultsView.ViewBag.displayvalue;
                    resultsView.ViewBag.runningresult = DoMath(resultsView.ViewBag.currentoperation, resultsView.ViewBag.runningresult, resultsView.ViewBag.prevdisplayvalue);
                    resultsView.ViewBag.displayvalue = String.Empty;
                    break;
                case calculate:
                    if (String.IsNullOrEmpty(resultsView.ViewBag.displayvalue.ToString()))
                    {
                        resultsView.ViewBag.runningresult = DoMath(resultsView.ViewBag.currentoperation, resultsView.ViewBag.runningresult, resultsView.ViewBag.prevdisplayvalue);
                    }
                    else
                    {
                        resultsView.ViewBag.prevdisplayvalue = resultsView.ViewBag.displayvalue;
                        resultsView.ViewBag.runningresult = DoMath(resultsView.ViewBag.currentoperation, resultsView.ViewBag.runningresult, resultsView.ViewBag.prevdisplayvalue);
                    }
                    resultsView.ViewBag.displayvalue = resultsView.ViewBag.runningresult;
                    resultsView.ViewBag.currentoperation = String.Empty;
                    break;

                default:
                    // remove the + submit when further along development...
                    resultsView.ViewBag.displayvalue = resultsView.ViewBag.displayvalue + submit;
                    break;

            }
            resultsView.ViewBag.recentrunning = resultsView.ViewBag.runningresult.ToString() + resultsView.ViewBag.currentoperation.ToString();
            return resultsView;
        }
        private String DoMath(String currentoperation, String runningresult, String displayvalue)
        {
            Decimal result = 0;
            Decimal runningvalue = 0;
            Decimal inputvalue = 0;

            Boolean parseresult = false;

            parseresult = Decimal.TryParse(runningresult, out runningvalue);
            // handle the false here...
            parseresult = Decimal.TryParse(displayvalue, out inputvalue);
            //handle the false here...

            switch (currentoperation)
            {
                case add:
                    result = runningvalue + inputvalue;
                    break;
                case subtract:
                    result = runningvalue - inputvalue;
                    break;
                case multiply:
                    result = runningvalue * inputvalue;
                    break;
                case divide:
                    result = runningvalue / (inputvalue != 0 ? inputvalue : 1);
                    break;
            }

            return result.ToString();
        }
    }
}