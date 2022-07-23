using System;
using System.Linq;
internal static class Program
{
    #region classes
    public class NoDados
    {
        public string Valor { get; set; }
        public NoDados[] filhos { get; set; }
        public NoDados()
        {
            filhos = new NoDados[2];
        }
    }
    #endregion

    static void Main()
    {
        string[,] exemplo1 = new string[,]{
                                        { "A", "B" }
                                        , { "A", "C" }
                                        , { "B", "G" }
                                        , { "C", "H" }
                                        , { "E", "F" }
                                        , { "B", "D" }
                                        , { "C", "E" }
                                        };

        string arvoreExemplo1 = organizaArray(exemplo1);
        Console.WriteLine("Exemplo 1: " + arvoreExemplo1);
        Console.WriteLine("");

        //precisa dar o erro E2
        string[,] exemplo2 = new string[,]{
                                        { "B", "D" }
                                        , { "D", "E" }
                                        , { "A", "B" }
                                        , { "C", "F" }
                                        , { "E", "G" }
                                        , { "A", "C" }
                                        };

        string arvoreExemplo2 = organizaArray(exemplo2);
        Console.WriteLine("Exemplo 2: " + arvoreExemplo2);
        Console.WriteLine("");

        //precisa dar o erro E3
        string[,] exemplo3 = new string[,]{
                                        { "A", "B" }
                                        , { "A", "C" }
                                        , { "B", "D" }
                                        , { "D", "C" }
                                        };

        string arvoreExemplo3 = organizaArray(exemplo3);
        Console.WriteLine("Exemplo 3: " + arvoreExemplo3);
        Console.WriteLine("");
    }

    #region metodos auxiliares
    public static string organizaArray(string[,] arrayDados)
    {
        try
        {
            //Cria um array de No de Dados aonde contem o Valor e um No de Dados de no MAX 2 filhos
            NoDados[] arrayOrganizado = new NoDados[(arrayDados.Length / 2)];

            int count = 0;
            //for passando pelos campos pai primeiro
            for (var i = 0; i < (arrayDados.Length / 2); i++)
            {
                bool paiJaExiste = false;
                //for para passar pelo array, validar se pai já existe no No de Dados
                for (var j = 0; j < i; j++)
                {
                    //validação para objeto nullo
                    if (arrayOrganizado[j] != null)
                    {
                        //Caso pai já existe entra no if e faz adicionar segundo filho.
                        if (arrayOrganizado[j].Valor == (arrayDados[i, 0]))
                        {
                            paiJaExiste = true;

                            if (arrayOrganizado[j].filhos[1] == null)
                            {
                                if (arrayOrganizado[j].filhos[0].Valor != arrayDados[i, 1])
                                {
                                    if (validarFilhoTemPai(arrayDados[i, 1], arrayOrganizado))
                                    {
                                        return "Raízes múltiplas";
                                    }
                                    arrayOrganizado[j].filhos[1] = new NoDados();
                                    arrayOrganizado[j].filhos[1].Valor = arrayDados[i, 1];
                                }
                                else
                                {
                                    //Ciclo presente - ciclo já existe no sistema, assim não podendo adicionar novamente.
                                    return "Ciclo presente";
                                }
                            }
                            else
                            {
                                //Mais de 2 filhos - Quando no array o pai tiver mais de 2 filhos.
                                return "Mais de 2 filhos";

                            }
                        }
                    }
                }
                //Caso pai não exista ainda adiciona ele e seu primeiro filho.
                if (!paiJaExiste)
                {
                    arrayOrganizado[count] = new NoDados();
                    arrayOrganizado[count].Valor = arrayDados[i, 0];

                    if (validarFilhoTemPai(arrayDados[i, 1], arrayOrganizado))
                    {
                        return "Raízes múltiplas";
                    }
                    arrayOrganizado[count].filhos[0] = new NoDados();
                    arrayOrganizado[count].filhos[0].Valor = arrayDados[i, 1];
                    count++;
                }
            }

            return retornaStringEstruturaHierarquicaOrganizado(arrayOrganizado);

        }
        catch
        {
            //Caso ocorre algum erro no processo cairia aqui, pode-se colocar a Exception dentro do catch
            return "Qualquer outro erro";
        }
    }

    private static bool validarFilhoTemPai(string valorFilho, NoDados[] arrayOrganizado)
    {
        //Validar se filho já tem pai, caso já tenha retorna true.
        foreach (NoDados item in arrayOrganizado)
        {
            if (item != null)
            {
                if (item.filhos[0] != null || item.filhos[1] != null)
                {
                    foreach (NoDados itemFilho in item.filhos)
                    {
                        if (itemFilho != null)
                        {
                            if (itemFilho.Valor == valorFilho)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                break;
            }
        }

        return false;
    }

    private static string retornaStringEstruturaHierarquicaOrganizado(NoDados[] arrayOrganizado)
    {
        string retorno = String.Empty;
        string organizaFilhos = String.Empty;

        //Removendo os campos nullos do array já organizado por pai e filhos.
        NoDados[] nDados = arrayOrganizado.Where(c => c != null).ToArray();

        //Monta a estrutura hierárquica conforme exercício 1 solicita o resultado. 
        foreach (NoDados noPai in nDados.OrderBy(c => c.Valor).ToArray())
        {
            if (noPai != null)
            {
                if (string.IsNullOrEmpty(retorno))
                {
                    foreach (NoDados noFilho in noPai.filhos)
                    {
                        if (noFilho != null)
                        {
                            retorno = retorno + "[" + noFilho.Valor + "]";
                        }
                    }
                    retorno = noPai.Valor + "[" + retorno + "]";

                }
                else if (retorno.Contains(noPai.Valor))
                {
                    organizaFilhos = string.Empty;

                    foreach (NoDados noFilho in noPai.filhos)
                    {
                        if (noFilho != null)
                        {
                            organizaFilhos = "[" + noFilho.Valor + "]" + organizaFilhos;
                        }
                    }

                    retorno = retorno.Replace(noPai.Valor, noPai.Valor + organizaFilhos);

                }
            }
        }

        return retorno;
    }
    #endregion
}