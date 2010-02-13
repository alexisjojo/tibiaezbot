using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using System.Xml;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using TibiaEzBot.Core.Util;
using TibiaEzBot.Core.Entities;

namespace TibiaEzBot.Core.Configs
{
    public static class ConfigManager
    {
        public static bool SaveConfig(Window mainWindow, String fileName)
        {
            try
            {
                FileStream stream = new FileStream(fileName, FileMode.Create);
                XmlTextWriter xmlWriter = new XmlTextWriter(stream, null);
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.Indentation = 4;

                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Configurations");

                xmlWriter.WriteComment("Specific configurations");

                if (Core.Kernel.GetInstance().AutoWalk.WaypointsFile != null)
                {
                    xmlWriter.WriteStartElement("Configuration");
                    xmlWriter.WriteAttributeString("type", "WaypointsFile");
                    xmlWriter.WriteAttributeString("value", Core.Kernel.GetInstance().AutoWalk.WaypointsFile);
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteComment("Generic configurations");

                foreach (TextBox textBox in TreeHelper.FindChildren<TextBox>(mainWindow))
                {
                    if (textBox.Tag != null)
                    {
                        xmlWriter.WriteStartElement("Configuration");
                        xmlWriter.WriteAttributeString("type", "TextBox");
                        xmlWriter.WriteAttributeString("id", (string)textBox.Tag);
                        xmlWriter.WriteAttributeString("value", textBox.Text);
                        xmlWriter.WriteEndElement();
                    }
                }

                foreach (ComboBox comboBox in TreeHelper.FindChildren<ComboBox>(mainWindow))
                {
                    if (comboBox.Tag != null)
                    {
                        xmlWriter.WriteStartElement("Configuration");
                        xmlWriter.WriteAttributeString("type", "ComboBox");
                        xmlWriter.WriteAttributeString("id", (string)comboBox.Tag);
                        xmlWriter.WriteAttributeString("value", comboBox.SelectedIndex.ToString());
                        xmlWriter.WriteEndElement();
                    }
                }

                foreach (CheckBox checkbox in TreeHelper.FindChildren<CheckBox>(mainWindow))
                {
                    if (checkbox.Tag != null)
                    {
                        xmlWriter.WriteStartElement("Configuration");
                        xmlWriter.WriteAttributeString("type", "CheckBox");
                        xmlWriter.WriteAttributeString("id", (string)checkbox.Tag);
                        xmlWriter.WriteAttributeString("value", checkbox.IsChecked.ToString());
                        xmlWriter.WriteEndElement();
                    }
                }

                foreach (MenuItem menuItem in TreeHelper.FindChildren<MenuItem>(mainWindow))
                {
                    if (menuItem.Tag != null)
                    {
                        xmlWriter.WriteStartElement("Configuration");
                        xmlWriter.WriteAttributeString("type", "MenuItem");
                        xmlWriter.WriteAttributeString("id", (string)menuItem.Tag);
                        xmlWriter.WriteAttributeString("value", menuItem.IsChecked.ToString());
                        xmlWriter.WriteEndElement();
                    }
                }

                foreach (Expander expander in TreeHelper.FindChildren<Expander>(mainWindow))
                {
                    if (expander.Tag != null)
                    {
                        xmlWriter.WriteStartElement("Configuration");
                        xmlWriter.WriteAttributeString("type", "Expander");
                        xmlWriter.WriteAttributeString("id", (string)expander.Tag);
                        xmlWriter.WriteAttributeString("value", expander.IsExpanded.ToString());
                        xmlWriter.WriteEndElement();
                    }
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
                stream.Close();

                return true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Save config error: " + e.Message,
                    "Error", System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }

            return false;
        }

        public static bool LoadConfig(Window mainWindow, String fileName)
        {
            try
            {
                var configs = from c in XElement.Load(fileName).Elements("Configuration")
                              select c;

                foreach (var config in configs)
                {
                    switch (config.Attribute("type").Value)
                    {
                        case "WaypointsFile":
                            String path = config.Attribute("value").Value;
                            if (File.Exists(path))
                            {
                                LoadWaypoints(path);
                            }
                            break;
                        case "TextBox":
                            TextBox textBox = TreeHelper.FindChildren<TextBox>(mainWindow).FirstOrDefault(
                                delegate(TextBox tb)
                                {
                                    return tb.Tag != null && ((string)tb.Tag).Equals(config.Attribute("id").Value);
                                });

                            if (textBox != null)
                            {
                                textBox.Text = config.Attribute("value").Value;
                            }
                            break;
                        case "ComboBox":
                            ComboBox comboBox = TreeHelper.FindChildren<ComboBox>(mainWindow).FirstOrDefault(
                                delegate(ComboBox cb)
                                {
                                    return cb.Tag != null && ((string)cb.Tag).Equals(config.Attribute("id").Value);
                                });

                            if (comboBox != null)
                            {
                                comboBox.SelectedIndex = Int32.Parse(config.Attribute("value").Value);
                            }

                            break;
                        case "CheckBox":
                            CheckBox checkBox = TreeHelper.FindChildren<CheckBox>(mainWindow).FirstOrDefault(
                                delegate(CheckBox cb)
                                {
                                    return cb.Tag != null && ((string)cb.Tag).Equals(config.Attribute("id").Value);
                                });

                            if (checkBox != null)
                            {
                                checkBox.IsChecked = Boolean.Parse(config.Attribute("value").Value);
                            }

                            break;
                        case "MenuItem":
                            MenuItem menuItem = TreeHelper.FindChildren<MenuItem>(mainWindow).FirstOrDefault(
                                delegate(MenuItem mi)
                                {
                                    return mi.Tag != null && ((string)mi.Tag).Equals(config.Attribute("id").Value);
                                });

                            if (menuItem != null)
                            {
                                menuItem.IsChecked = Boolean.Parse(config.Attribute("value").Value);
                            }
                            break;
                        case "Expander":
                            Expander expander = TreeHelper.FindChildren<Expander>(mainWindow).FirstOrDefault(
                                delegate(Expander ex)
                                {
                                    return ex.Tag != null && ((string)ex.Tag).Equals(config.Attribute("id").Value);
                                });

                            if (expander != null)
                            {
                                expander.IsExpanded = Boolean.Parse(config.Attribute("value").Value);
                            }
                            break;
                    }
                }

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Load config error: " + e.Message,
                    "Error", System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }


            return false;
        }

        public static bool LoadWaypoints(String fileName)
        {
            try
            {
                Core.Kernel.GetInstance().AutoWalk.Clear();

                var waypoints = from w in XElement.Load(fileName).Elements("Waypoint")
                                select w;

                foreach (var waypoint in waypoints)
                {
                    WaypointType type = (WaypointType)int.Parse(waypoint.Attribute("type").Value);
                    switch (type)
                    {
                        case WaypointType.WAYPOINT_GROUND:
                        case WaypointType.WAYPOINT_HOLE:
                        case WaypointType.WAYPOINT_LADDER:
                        case WaypointType.WAYPOINT_RAMP:
                        case WaypointType.WAYPOINT_ROPE:
                        case WaypointType.WAYPOINT_STAIR_UP:
                        case WaypointType.WAYPOINT_START_DOWN:
                            Core.Kernel.GetInstance().AutoWalk.AddWaypoint(new WalkWaypoint(
                                new Position(uint.Parse(waypoint.Attribute("x").Value),
                                    uint.Parse(waypoint.Attribute("y").Value),
                                    uint.Parse(waypoint.Attribute("z").Value)), type));
                            break;
                        case WaypointType.WAYPOINT_SAY:
                            Core.Kernel.GetInstance().AutoWalk.AddWaypoint(waypoint.Attribute("words").Value,
                                (SayType)byte.Parse(waypoint.Attribute("sayType").Value));
                            break;
                        case WaypointType.WAYPOINT_DELAY:
                            Core.Kernel.GetInstance().AutoWalk.AddWaypoint(int.Parse(waypoint.Attribute("type").Value));
                            break;
                    }
                }

                Core.Kernel.GetInstance().AutoWalk.WaypointsFile = fileName;
                return true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Load waypoints error: " + e.Message,
                    "Error", System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }

            return false;
        }

        public static bool SaveWaypoints(String fileName)
        {
            try
            {
                FileStream stream = new FileStream(fileName, FileMode.Create);
                XmlTextWriter xmlWriter = new XmlTextWriter(stream, null);
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.Indentation = 4;

                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Waypoints");

                foreach (Waypoint waypoint in Core.Kernel.GetInstance().AutoWalk.Waypoints)
                {
                    switch (waypoint.WaypointType)
                    {
                        case WaypointType.WAYPOINT_GROUND:
                        case WaypointType.WAYPOINT_HOLE:
                        case WaypointType.WAYPOINT_LADDER:
                        case WaypointType.WAYPOINT_RAMP:
                        case WaypointType.WAYPOINT_ROPE:
                        case WaypointType.WAYPOINT_STAIR_UP:
                        case WaypointType.WAYPOINT_START_DOWN:
                            xmlWriter.WriteStartElement("Waypoint");
                            xmlWriter.WriteAttributeString("type", ((int)waypoint.WaypointType).ToString());
                            xmlWriter.WriteAttributeString("x", waypoint.GetWalkWaypoint().Position.X.ToString());
                            xmlWriter.WriteAttributeString("y", waypoint.GetWalkWaypoint().Position.Y.ToString());
                            xmlWriter.WriteAttributeString("z", waypoint.GetWalkWaypoint().Position.Z.ToString());
                            xmlWriter.WriteEndElement();
                            break;
                        case WaypointType.WAYPOINT_SAY:
                            xmlWriter.WriteStartElement("Waypoint");
                            xmlWriter.WriteAttributeString("type", ((int)waypoint.WaypointType).ToString());
                            xmlWriter.WriteAttributeString("words", waypoint.GetSayWaypoint().Words);
                            xmlWriter.WriteAttributeString("sayType", ((byte)waypoint.GetSayWaypoint().SayType).ToString());
                            xmlWriter.WriteEndElement();
                            break;
                        case WaypointType.WAYPOINT_DELAY:
                            xmlWriter.WriteStartElement("Waypoint");
                            xmlWriter.WriteAttributeString("type", ((int)waypoint.WaypointType).ToString());
                            xmlWriter.WriteAttributeString("time", waypoint.GetWaitWaypoint().Delay.ToString());
                            xmlWriter.WriteEndElement();
                            break;
                    }
                }


                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
                stream.Close();

                Core.Kernel.GetInstance().AutoWalk.WaypointsFile = fileName;
                return true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Save waypoints error: " + e.Message,
                    "Error", System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }

            return false;
        }

    }
}
