using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Rutas de los archivos
            string entradaPath = "C:\\Users\\ktajt\\Desktop\\Entrada.txt";
            string expresionesPath = "C:\\Users\\ktajt\\Desktop\\Expresion.txt";
            string salidaPath = "C:\\Users\\ktajt\\Desktop\\Salida.txt";

            // Leer el archivo de expresiones regulares
            var expresiones = File.ReadLines(expresionesPath)
                .Select(line =>
                {
                    var parts = line.Split(new[] { '=' }, 2);
                    if (parts.Length == 2)
                    {
                        string nombre = parts[0].Trim();
                        string regex = parts[1].Trim().Trim('"').Replace("\\\\", "\\");
                        return new Tuple<string, Regex>(nombre, new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase));
                    }
                    else
                    {
                        Console.WriteLine("Formato inválido en la línea: " + line);
                        return null; // Para manejar el caso de líneas inválidas
                    }
                })
                .Where(tuple => tuple != null) // Filtrar las líneas válidas
                .ToList(); // Convertir a lista

            // Leer el archivo de entrada y clasificar cadenas
            var salida = new List<string>();

            foreach (var cadena in File.ReadLines(entradaPath))
            {
                string trimmedCadena = cadena.Trim();
                bool matched = false;

                // Usar LINQ para encontrar la primera coincidencia
                var coincidencia = expresiones.FirstOrDefault(expresion =>
                {
                    Console.WriteLine($"Probando {trimmedCadena} con expresión {expresion.Item2}");
                    return expresion.Item2.IsMatch(trimmedCadena);
                });

                if (coincidencia != null)
                {
                    salida.Add($"{trimmedCadena} - {coincidencia.Item1}");
                    matched = true;
                }

                if (!matched)
                {
                    salida.Add($"{trimmedCadena} - ERROR");
                }
            }

            // Escribir los resultados en el archivo de salida
            File.WriteAllLines(salidaPath, salida);

            Console.WriteLine("Proceso completado. Archivo de salida generado.");
        }
    }
}