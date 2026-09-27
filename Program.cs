using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

class Grafo
{
    private Dictionary<string, List<string>> adyacencia;
    private List<(string Origen, string Destino)> conexiones;

    public Grafo()
    {
        adyacencia = new Dictionary<string, List<string>>();
        conexiones = new List<(string Origen, string Destino)>();
    }

    public void AgregarVertice(string vertice)
    {
        if (!adyacencia.ContainsKey(vertice))
        {
            adyacencia[vertice] = new List<string>();
        }
    }

    public void AgregarConexion(string origen, string destino)
    {
        AgregarVertice(origen);
        AgregarVertice(destino);

        if (!adyacencia[origen].Contains(destino))
        {
            adyacencia[origen].Add(destino);
        }

        if (!adyacencia[destino].Contains(origen))
        {
            adyacencia[destino].Add(origen);
        }

        bool existe = false;

        foreach (var conexion in conexiones)
        {
            if ((conexion.Origen == origen && conexion.Destino == destino) ||
                (conexion.Origen == destino && conexion.Destino == origen))
            {
                existe = true;
                break;
            }
        }

        if (!existe)
        {
            conexiones.Add((origen, destino));
        }
    }

    public void MostrarListaAdyacencia()
    {
        Console.WriteLine("\nLISTA DE ADYACENCIA");

        foreach (var vertice in adyacencia)
        {
            Console.Write(vertice.Key + " -> ");

            if (vertice.Value.Count == 0)
            {
                Console.WriteLine("Sin conexiones");
            }
            else
            {
                Console.WriteLine(string.Join(", ", vertice.Value));
            }
        }
    }

    public void MostrarGrafo()
    {
        Console.WriteLine("\nCONEXIONES DEL GRAFO");

        foreach (var conexion in conexiones)
        {
            Console.WriteLine(
                $"{conexion.Origen} -------- {conexion.Destino}"
            );
        }
    }

    public bool ExisteConexion(string origen, string destino)
    {
        return adyacencia.ContainsKey(origen) &&
               adyacencia[origen].Contains(destino);
    }

    public int CantidadVertices()
    {
        return adyacencia.Count;
    }

    public int CantidadConexiones()
    {
        return conexiones.Count;
    }

    public void MostrarReporte()
    {
        Console.WriteLine("\n========== REPORTERÍA ==========");

        Console.WriteLine($"Cantidad de vértices: {CantidadVertices()}");
        Console.WriteLine($"Cantidad de conexiones: {CantidadConexiones()}");

        MostrarListaAdyacencia();

        Console.WriteLine("\nCONEXIONES DEL GRAFO");

        foreach (var conexion in conexiones)
        {
            Console.WriteLine(
                $"{conexion.Origen} -------- {conexion.Destino}"
            );
        }

        Console.WriteLine("================================");
    }
}

class Program
{
    static Grafo LeerArchivo(string archivo)
    {
        Grafo grafo = new Grafo();

        string[] lineas = File.ReadAllLines(archivo);

        foreach (string linea in lineas)
        {
            string texto = linea.Trim();

            if (string.IsNullOrWhiteSpace(texto))
                continue;

            if (texto.StartsWith("["))
                continue;

            string[] partes = texto.Split(',');

            if (partes.Length == 2)
            {
                string origen = partes[0].Trim();
                string destino = partes[1].Trim();

                grafo.AgregarConexion(origen, destino);
            }
        }

        return grafo;
    }

    static void Main()
    {
        string archivo = "datos_grafos.txt";

        if (!File.Exists(archivo))
        {
            Console.WriteLine("No se encontró el archivo datos_grafos.txt");
            Console.WriteLine(
                "Verifique que esté en la misma carpeta del programa."
            );
            return;
        }

        string[] lineas = File.ReadAllLines(archivo);

        List<string> grafo1Lineas = new List<string>();
        List<string> grafo2Lineas = new List<string>();

        bool grafo1 = false;
        bool grafo2 = false;

        foreach (string linea in lineas)
        {
            string texto = linea.Trim();

            if (texto == "[GRAFO1]")
            {
                grafo1 = true;
                grafo2 = false;
                continue;
            }

            if (texto == "[GRAFO2]")
            {
                grafo1 = false;
                grafo2 = true;
                continue;
            }

            if (grafo1 && !string.IsNullOrWhiteSpace(texto))
            {
                grafo1Lineas.Add(texto);
            }

            if (grafo2 && !string.IsNullOrWhiteSpace(texto))
            {
                grafo2Lineas.Add(texto);
            }
        }

        Grafo redCiudades = new Grafo();
        Grafo redComputadores = new Grafo();

        foreach (string linea in grafo1Lineas)
        {
            string[] partes = linea.Split(',');

            if (partes.Length == 2)
            {
                redCiudades.AgregarConexion(
                    partes[0].Trim(),
                    partes[1].Trim()
                );
            }
        }

        foreach (string linea in grafo2Lineas)
        {
            string[] partes = linea.Split(',');

            if (partes.Length == 2)
            {
                redComputadores.AgregarConexion(
                    partes[0].Trim(),
                    partes[1].Trim()
                );
            }
        }

        Stopwatch tiempo = Stopwatch.StartNew();

        int opcion;

        do
        {
            Console.Clear();

            Console.WriteLine("======================================");
            Console.WriteLine("          GRÁFICA DE GRAFOS");
            Console.WriteLine("======================================");
            Console.WriteLine("1. Mostrar Grafo 1 - Red de ciudades");
            Console.WriteLine("2. Mostrar Grafo 2 - Red de computadores");
            Console.WriteLine("3. Mostrar reportería Grafo 1");
            Console.WriteLine("4. Mostrar reportería Grafo 2");
            Console.WriteLine("5. Consultar conexión Grafo 1");
            Console.WriteLine("6. Consultar conexión Grafo 2");
            Console.WriteLine("7. Mostrar tiempo de ejecución");
            Console.WriteLine("8. Salir");
            Console.WriteLine("======================================");
            Console.Write("Seleccione una opción: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                Console.WriteLine("\nIngrese una opción válida.");
                Console.ReadKey();
                continue;
            }

            switch (opcion)
            {
                case 1:
                    Console.Clear();

                    Console.WriteLine(
                        "===== GRAFO 1: RED DE CIUDADES ====="
                    );

                    redCiudades.MostrarGrafo();

                    Console.ReadKey();
                    break;

                case 2:
                    Console.Clear();

                    Console.WriteLine(
                        "===== GRAFO 2: RED DE COMPUTADORES ====="
                    );

                    redComputadores.MostrarGrafo();

                    Console.ReadKey();
                    break;

                case 3:
                    Console.Clear();

                    Console.WriteLine(
                        "===== REPORTERÍA GRAFO 1 ====="
                    );

                    redCiudades.MostrarReporte();

                    Console.ReadKey();
                    break;

                case 4:
                    Console.Clear();

                    Console.WriteLine(
                        "===== REPORTERÍA GRAFO 2 ====="
                    );

                    redComputadores.MostrarReporte();

                    Console.ReadKey();
                    break;

                case 5:
                    Console.Clear();

                    Console.WriteLine("===== CONSULTA GRAFO 1 =====");

                    Console.Write("Ingrese ciudad de origen: ");
                    string origen1 = Console.ReadLine() ?? "";

                    Console.Write("Ingrese ciudad de destino: ");
                    string destino1 = Console.ReadLine() ?? "";

                    if (redCiudades.ExisteConexion(origen1, destino1))
                    {
                        Console.WriteLine(
                            $"\nSí existe conexión entre {origen1} y {destino1}."
                        );
                    }
                    else
                    {
                        Console.WriteLine(
                            $"\nNo existe conexión entre {origen1} y {destino1}."
                        );
                    }

                    Console.ReadKey();
                    break;

                case 6:
                    Console.Clear();

                    Console.WriteLine("===== CONSULTA GRAFO 2 =====");

                    Console.Write("Ingrese computador de origen: ");
                    string origen2 = Console.ReadLine() ?? "";

                    Console.Write("Ingrese computador de destino: ");
                    string destino2 = Console.ReadLine() ?? "";

                    if (redComputadores.ExisteConexion(origen2, destino2))
                    {
                        Console.WriteLine(
                            $"\nSí existe conexión entre {origen2} y {destino2}."
                        );
                    }
                    else
                    {
                        Console.WriteLine(
                            $"\nNo existe conexión entre {origen2} y {destino2}."
                        );
                    }

                    Console.ReadKey();
                    break;

                case 7:
                    tiempo.Stop();

                    Console.Clear();

                    Console.WriteLine(
                        "===== TIEMPO DE EJECUCIÓN ====="
                    );

                    Console.WriteLine(
                        $"Tiempo transcurrido: {tiempo.Elapsed.TotalMilliseconds:F4} ms"
                    );

                    Console.ReadKey();

                    tiempo.Start();
                    break;

                case 8:
                    Console.WriteLine("\nPrograma finalizado.");
                    break;

                default:
                    Console.WriteLine("\nOpción no válida.");
                    Console.ReadKey();
                    break;
            }

        } while (opcion != 8);
    }
}
