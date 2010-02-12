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

namespace TibiaEzBot.Configs
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

                xmlWriter.WriteComment("Specific configurations");


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
    }
}
