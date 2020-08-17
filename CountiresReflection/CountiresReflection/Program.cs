using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CoutriesReflect
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.LoadFrom("Countries.dll");
            Type[] countryTypes = assembly.GetTypes();
            PrintInterfaceInfo(assembly);
            PrintCountryDetails(countryTypes, assembly);
            PrintTopSecretMethods(countryTypes);
            PrintContrysContinental(countryTypes, assembly, "mamas");

        }

        /// <summary>
        /// --- Exercise 3b ---
        /// </summary>
        /// <param name="countryTypes"></param>
        public static void PrintTopSecretMethods(Type[] countryTypes)
        {
            if (countryTypes == null)
            {
                Console.WriteLine("No Countries");
                return;
            }
            Console.WriteLine("Top Secret Methods:");
            Console.WriteLine("==============================");
            Type topSecretAttr = countryTypes.Where(x => x.Name == "TopSecretAttribute").First();
            foreach (var t in countryTypes)
            {
                if (t.FullName.Contains("Countries.Countries"))
                {
                    var methods = t.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
                    Console.WriteLine($"{t.FullName}: ");
                    foreach (var method in methods)
                    {

                        var customAttrArray = method.GetCustomAttributesData();
                        foreach (var attr in customAttrArray)
                        {
                            if (attr.AttributeType.Name.Equals(topSecretAttr.Name))
                                Console.WriteLine(method.Name);
                        }


                    }
                    Console.WriteLine("==============================");
                }
            }
        }
        /// <summary>
        /// --- Exercise 2 ---
        /// Method gets a types array and the assembly.
        /// For every type, checks if it is a country.
        /// If it is, prints it`s Name, population, and size.
        /// </summary>
        /// <param name="countryTypes"></param>
        /// <param name="assembly"></param>
        public static void PrintCountryDetails(Type[] countryTypes, Assembly assembly)
        {
            if (countryTypes == null)
            {
                Console.WriteLine("No Countries");
                return;

            }
            foreach (var t in countryTypes)
            {
                if (t.FullName.Contains("Countries.Countries"))

                {
                    object country = assembly.CreateInstance(t.FullName);
                    MethodInfo getNameMethod = t.GetMethod("get_Name");
                    MethodInfo getPopulationMethod = t.GetMethod("get_Population");
                    MethodInfo calculateSizeMethod = t.GetMethod("CalculateSize");
                    if (country != null)
                    {
                        var nameResult = getNameMethod.Invoke(country, null);
                        var populationResult = getPopulationMethod.Invoke(country, null);
                        var sizeResult = calculateSizeMethod.Invoke(country, null);
                        Console.WriteLine($"{t.Name}:");
                        Console.WriteLine($"Name: {nameResult}");
                        Console.WriteLine($"Population: {populationResult}");
                        Console.WriteLine($"Size: {sizeResult}");
                        Console.WriteLine("==========================");
                    }
                    else { Console.WriteLine("No such method/country"); }
                }
            }
        }
        /// <summary>
        /// --- Exercise 1 ---
        /// Method gets the assembly, and prints all members of ICountry interface
        /// </summary>
        /// <param name="assembly"></param>
        public static void PrintInterfaceInfo(Assembly assembly)
        {
            var countryInterface = assembly.GetType("Countries.Interfaces.ICountry");
            var methods = countryInterface.GetMethods();
            var fields = countryInterface.GetFields();
            var props = countryInterface.GetProperties();
            foreach (var m in methods)
            {
                Console.WriteLine($"Member Type: {m.MemberType}, Name: {m.Name}");
            }
            foreach (var f in fields)
            {
                Console.WriteLine($"Member Type: {f.MemberType}, Name: {f.Name}");
            }
            foreach (var p in props)
            {
                Console.WriteLine($"Member Type: {p.MemberType}, Name: {p.Name}");
            }
        }


        /// <summary>
        /// --- Excercise 3c ---
        /// </summary>
        /// <param name="countryTypes"></param>
        /// <param name="assembly"></param>
        /// <param name="countryName"></param>
        public static void PrintContrysContinental(Type[] countryTypes, Assembly assembly, string countryName)
        {
            var country = countryTypes.Where(c => c.Name.ToLower().Contains(countryName)).ToList().First();
            var countryAttr = System.Attribute.GetCustomAttributes(country);
            var continentAttributeType = country.CustomAttributes.ToList();
            foreach (var attr in continentAttributeType)
            {
                if (attr.AttributeType.FullName.ToLower().Contains("continent"))
                    Console.WriteLine(String.Join(", ", attr.ConstructorArguments));
            }
        }
    }
}