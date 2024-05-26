using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Linq;
namespace Cifras
{
    public partial class Form1 : Form
    {
       
        Dictionary<int, string> letrasComNumero = new Dictionary<int, string>();
        PrivateFontCollection fontCollection = new PrivateFontCollection();
        List<Image> animation;
        Dictionary<char, string> morseFiles = new Dictionary<char, string>();
        Teclado_Angular tc = new Teclado_Angular();


        void ConstruirCodigoMorse()
        {
            
            DirectoryInfo directoryInfo = new DirectoryInfo(@"Codigo morse");
            List<FileInfo> fi = new List<FileInfo>();
            fi.AddRange(directoryInfo.GetFiles());
            string letras = "abcdefghijklmnopqrstuvwxyz";
            foreach(char c in letras)
            {
                string cUpper = c.ToString().ToUpper();
                string filename = fi.Find(x => x.FullName.Contains("Morse Code Alphabet " + cUpper)).FullName;
                morseFiles.Add(c, filename);
               
            }
            string numeros = "0123456789";
            foreach(char c in numeros)
            {
                string filename = fi.Find(x => x.FullName.Contains("numero" + c)).FullName;
                morseFiles.Add(c, filename);
            }
            
        }
        string[,,] quadrados = new string[,,]
        {
            {
                {"a","b","c"},
                {"d","e","f"},
                {"g","h","i"}

            },

            {
                {"j","k","l"},
                {"m","n","o" },
                {"p","q","r" }
            },

            {
                {"s","t","u"},
                {"v","w","x"},
                {"y","z"," "}
            }
        };






        public Tuple<int,int,int> GetCharIndexes(string character)
        {
            int quadrado = 0;
            int linha = 0;
            int coluna = 0;
            for(int i = 0; i < 3;i++)
            {
                for(int j = 0; j < 3;j++)
                {
                    for(int k = 0; k < 3;k++)
                    {
                        if(quadrados[i,j,k] == character.ToLower())
                        {
                            quadrado = i;
                            linha = j;
                            coluna = k;
                            goto paraAqui;
                        }
                    }
                }
            }

paraAqui:  return new Tuple<int, int, int>(quadrado, linha, coluna);
        }

        public Form1()
        {
            InitializeComponent();
            iniciarDicionario();
            fontCollection.AddFontFile(@"Fontes/codigochines.ttf");
            fontCollection.AddFontFile(@"Fontes/braille-falso.ttf");
            Font font = new Font(fontCollection.Families[1], 30);
            rtxt_encriptado.Font = font;
            txt_braille_encriptado.Font = new Font(fontCollection.Families[0], 15);
            ConstruirCodigoMorse();
            tc.txtInput = txt_desencriptado_angular;
            
            panel1.Controls.Add(tc);
            
            
            

            
            
            
        }

        void iniciarDicionario()
        {
            letrasComNumero.Add(1, "a");
            letrasComNumero.Add(2, "b");
            letrasComNumero.Add(3, "c");
            letrasComNumero.Add(4, "d");
            letrasComNumero.Add(5, "e");
            letrasComNumero.Add(6, "f");
            letrasComNumero.Add(7, "g");
            letrasComNumero.Add(8, "h");
            letrasComNumero.Add(9, "i");
            letrasComNumero.Add(0, "j");
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        
        public void Animacao()
        {
            foreach(Image img in animation)
            {
                pic_output.Image = img;
                Thread.Sleep(2000);
            }
        }
        private void btn_converter_Click(object sender, EventArgs e)
        {
            animation = new List<Image>();
            string input = txt_input.Text.ToLower();
            
            foreach(char c in input)
            {
                string suffix = "";
                string diretorio = @"Imagens";
                suffix = "a";
                string[] files = Directory.GetFiles(diretorio);
                List<string> listFiles = new List<string>(files);
               if(c == ' ')
                {
                    Image img = Image.FromFile(listFiles.Find(x => x.Contains("fim da palavra.PNG")));
                    animation.Add(img);
                    continue;
                }



                if (char.IsLetter(c))
                {
                    
                    
                    int num = -1;
                    if(letrasComNumero.ContainsValue(c.ToString()))
                    {
                        foreach(int numero in letrasComNumero.Keys)
                        {
                            if(letrasComNumero[numero] == c.ToString())
                            {
                                num = numero;
                                suffix += "n" + c.ToString() + num.ToString() + ".png";
                                break;
                            }
                        }


                    }
                    else
                    {
                        suffix += c.ToString() + ".png";
                    }

                    Image img = Image.FromFile(listFiles.Find(x => x.Contains(suffix)));
                    animation.Add(img);
                    

                }
                else if(char.IsNumber(c))
                {
                    Image eNumero = Image.FromFile(listFiles.Find(x => x.Contains("numeros.PNG")));
                    suffix += "n" + letrasComNumero[int.Parse(c.ToString())] + c.ToString() + ".png";
                    Image img = Image.FromFile(listFiles.Find(x => x.Contains(suffix)));
                    animation.Add(eNumero);
                    animation.Add(img);
                }
            }

            Thread thread = new Thread(Animacao);
            thread.Start();

        }

        private void btn_encriptar_Click(object sender, EventArgs e)
        {
            string[] cifra = File.ReadAllLines(@"Dicionario/Alfabeto_fonetico.dat");
            Dictionary<string, string> alfabeto_fonetico = new Dictionary<string, string>();
            foreach(string s in cifra)
            {
                string[] correspondencia = s.Split('-');
                alfabeto_fonetico.Add(correspondencia[0], correspondencia[1]);
            }

            string normal = txt_normal.Text;
            StringBuilder stringBuilder = new StringBuilder();
            foreach(char c in normal)
            {
                if(char.IsLetter(c))
                {
                    string cUpper = c.ToString().ToUpper();
                    string palavra = alfabeto_fonetico[cUpper];
                    stringBuilder.AppendLine(palavra);
                }
                
            }

            txt_encriptado_alfabeto.Text = stringBuilder.ToString();


            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rtxt_encriptado.Text = txt_texto_encriptar_cifra_chinesa.Text.ToLower();
        }

        private void rtxt_encriptado_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            txt_braille_encriptado.Text = txt_aEncriptar_Braille.Text;

            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if((char.IsLetter(e.KeyChar) || char.IsSymbol(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || char.IsPunctuation(e.KeyChar)) && char.GetNumericValue(e.KeyChar) != 8)
            {
                e.Handled = true;
            }
        }

        private void btn_encriptar_data_Click(object sender, EventArgs e)
        {
            List<int> indexes = new List<int>();
            txt_encriptado_data.Text = "";
            foreach(char c in txt_data.Text)
            {
                int i = int.Parse(c.ToString());
                indexes.Add(i);
            }
            Dictionary<int, List<char>> dataCifra = new Dictionary<int, List<char>>();
            string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            int j = 0;
            foreach(int index in indexes)
            {
                List<char> listaChars = new List<char>();
                for (; j < chars.Length; j++)
                {
                    listaChars.Add(chars[j]);
                    if (j + 1 == 10 || j + 1 == 20 || j+1 == 30)
                    {
                        j = j + 1;
                        break;
                    }

                }

               

                dataCifra.Add(index, listaChars);
                
                      
            }

            foreach(char c in txt_texto.Text.ToLower())
            {
                int identificador = -1;
                int indexLista = -1;
                int valorAColocar = -1;
                if(c == ' ')
                {
                    txt_encriptado_data.Text += ' ';
                    continue;
                }
                foreach (int index in indexes)
                {
                    foreach(char caractere in dataCifra[index])
                    {
                        if(c == caractere)
                        {
                            
                            identificador = index;


                            
                            if (dataCifra[index].IndexOf(c) == 9)
                            {
                                valorAColocar = 0;
                            }
                            else
                            {
                                valorAColocar = dataCifra[index].IndexOf(c) + 1;
                            }
                           
                            goto sair;


                        }
                    }
                }

            sair: txt_encriptado_data.Text += identificador.ToString() + valorAColocar.ToString();
            }
        }

        private void btn_encriptar_invertido_Click(object sender, EventArgs e)
        {
            string alfabeto = "abcdefghijklmnopqrstuvwxyz";
            List<char> alfabetoInvertido = new List<char>();
            alfabetoInvertido.AddRange(alfabeto.ToCharArray());
            alfabetoInvertido.Reverse();
            Dictionary<char, char> cifra = new Dictionary<char, char>();
            Encoding destEncoding = Encoding.GetEncoding("iso-8859-8");
            txt_encriptar_invertido.Text = destEncoding.GetString(Encoding.Convert(Encoding.UTF8, destEncoding, Encoding.UTF8.GetBytes(txt_encriptar_invertido.Text)));
            for (int i = 0; i < alfabeto.Length;i++)
            {
                cifra.Add(alfabeto[i], alfabetoInvertido[i]);
            }
            foreach (char c in txt_encriptar_invertido.Text.ToLower())
            {
                if(c == ' ')
                {
                    txt_encriptado_invertido.Text += " ";
                    continue;
                }
                txt_encriptado_invertido.Text += cifra[c];
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {

           
        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            List<int> indexes = new List<int>();
            txt_data_desencriptado.Text = "";
            foreach (char c in txt_decryptor_data.Text)
            {
                int i = int.Parse(c.ToString());
                indexes.Add(i);
            }
            /*foreach(int i in indexes)
            {
                Console.WriteLine(i);
            }
            */
            
            Dictionary<int, List<char>> dataCifra = new Dictionary<int, List<char>>();
            string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            int j = 0;

            foreach (int index in indexes)
            {
                List<char> listaChars = new List<char>();
                for (; j < chars.Length; j++)
                {
                    listaChars.Add(chars[j]);
                    if (j + 1 == 10 || j + 1 == 20 || j + 1 == 30)
                    {
                        j = j + 1;
                        break;
                    }

                }



                dataCifra.Add(index, listaChars);


            }

           /*foreach(int index in indexes)
           {
                for(int i = 0; i < dataCifra[index].Count;i++)
                {
                    Console.WriteLine("Linha("+index+","+(i+1)+")" + "- " + dataCifra[index][i]);
                }
           }
           */

            string encriptada = txt_decryptor_texto.Text;
            string[] array = encriptada.Split(' ');
            List<string> palavras = new List<string>();
            palavras.AddRange(array);
            foreach (string palavra in palavras)
            {
                int pares = palavra.Length / 2;
                encriptada = palavra;

                List<string> divided = new List<string>();
                for (int i = 0; i < pares; i++)
                {
                    string par = "";

                    for (int k = 0; k < 2; k++)
                    {
                        par += encriptada[k];
                    }
                    encriptada = encriptada.Remove(0, 2);
                    divided.Add(par);
                }
                string desencriptado = "";
                


                foreach (string par in divided)
                {
                    int linha = int.Parse(par[0].ToString());
                    int coluna = int.Parse(par[1].ToString()) - 1;
                    if (coluna == -1)
                    {
                        coluna = 9;
                    }

                    char c = dataCifra[linha][coluna];
                    desencriptado += c;
                }
                string espaco = " ";
                if(palavra.IndexOf(palavra) +1 == palavras.Count)
                {
                    txt_data_desencriptado.Text += desencriptado;
                }
                else
                {
                    txt_data_desencriptado.Text += desencriptado + espaco;
                }
                
            }
            
            
        }

        private void btn_encriptar_an_Click(object sender, EventArgs e)
        {
            
            int numero = int.Parse(txt_primeira_letra.Text);
            int numeroInicial = numero;
            int numeroFinal = -1;
            string alfabeto = "abcdefghijklmnopqrstuvwxyz";
            numeroFinal = numeroInicial + alfabeto.Length;
            Dictionary<int, char> cifra = new Dictionary<int, char>();
            foreach(char c in alfabeto)
            {
                cifra.Add(numero, c);
                numero++;
                
            }
            foreach(char c in txt_normal_an.Text.ToLower())
            {
                for(int i = numeroInicial; i <= numeroFinal;i++)
                {
                    if(c == cifra[i])
                    {
                        txt_encriptado_an.Text += i+ " ";
                        break;
                    }
                }
            }
        }

        void TocarMorse()
        {
            Encoding destEncoding = Encoding.GetEncoding("iso-8859-8");
            this.Invoke((MethodInvoker)delegate
            {
                txt_normal_morse.Text = destEncoding.GetString(Encoding.Convert(Encoding.UTF8, destEncoding, Encoding.UTF8.GetBytes(txt_normal_morse.Text)));
            });
            
            foreach (char c in txt_normal_morse.Text)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    listBox1.Items.Add(c);
                });
            }
            string str = txt_normal_morse.Text.ToLower();
            for(int i = 0; i < txt_normal_morse.Text.ToLower().Length;i++)
            {
                
                this.Invoke((MethodInvoker)delegate
                {
                    listBox1.SetSelected(i, true);
                });
                if(txt_normal_morse.Text.ToLower()[i] == ' ')
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        listBox1.SetSelected(i, false);
                    });
                    continue;
                }

                string filename = morseFiles[txt_normal_morse.Text.ToLower()[i]];
                md_morse.URL = filename;
                md_morse.Ctlcontrols.play();
                Thread.Sleep(2000);
                this.Invoke((MethodInvoker)delegate
                {
                    listBox1.SetSelected(i, false);
                });
            }
        }

        private void button1_Click_4(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            Thread thread = new Thread(TocarMorse);
            thread.Start();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txt_encriptado_ra.Text = "";
            Dictionary<char, string> vogais = new Dictionary<char, string>();
            vogais.Add('a',"I");
            vogais.Add('e', "II");
            vogais.Add('i', "III");
            vogais.Add('o', "IV");
            vogais.Add('u', "V");
            Dictionary<char, int> consoantes = new Dictionary<char, int>();
            string consoantesStr = "bcdfghjklmnpqrstvwxyz";
            for(int i = 0; i < consoantesStr.Length;i++)
            {
                consoantes.Add(consoantesStr[i], i + 1);
            }
            string normal = txt_normal_ra.Text.ToLower();
            foreach(char c in normal)
            {
                if(c == ' ')
                {
                    txt_encriptado_ra.Text += " ";
                    continue;
                }
                if(vogais.ContainsKey(c))
                {
                    txt_encriptado_ra.Text += vogais[c]+" ";
                }
                else
                {
                    txt_encriptado_ra.Text += consoantes[c]+" ";
                }
            }
            txt_encriptado_ra.Text = txt_encriptado_ra.Text.TrimEnd(' ');

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(txt_letra.Text.Length == 0)
            {
                MessageBox.Show("Digite a letra inicial");
                txt_letra.Focus();
                return;
            }
            char primeiraLetra = Convert.ToChar(txt_letra.Text);
            string alfabeto = "abcdefghijklmnopqrstuvwxyz";
            int primeiraLetraIndex = alfabeto.IndexOf(primeiraLetra);
            string transposto = "";
            for(int i = primeiraLetraIndex; i < alfabeto.Length;i++)
            {
                transposto += alfabeto[i];
            }
            for(int i = 0; i < primeiraLetraIndex;i++)
            {
                transposto += alfabeto[i];
            }
            Dictionary<char, char> dictionary = new Dictionary<char, char>();
            for(int i = 0; i < alfabeto.Length;i++)
            {
                dictionary.Add(alfabeto[i],transposto[i]);

            }
            Encoding destEncoding = Encoding.GetEncoding("iso-8859-8");
            txt_encriptar_at.Text = destEncoding.GetString(Encoding.Convert(Encoding.UTF8, destEncoding, Encoding.UTF8.GetBytes(txt_encriptar_at.Text)));
            txt_encriptado_at.Text = "";
            foreach (char c in txt_encriptar_at.Text.ToLower())
            {
                if(c == ' ')
                {
                    txt_encriptado_at.Text += " ";
                    continue;
                }
                txt_encriptado_at.Text += dictionary[c];
            }

        }

        private void txt_letra_TextChanged(object sender, EventArgs e)
        { 
            Encoding destEncoding = Encoding.GetEncoding("iso-8859-8");
            txt_letra.Text = destEncoding.GetString(Encoding.Convert(Encoding.UTF8, destEncoding, Encoding.UTF8.GetBytes(txt_letra.Text)));
            txt_letra.Text = txt_letra.Text.ToLower();
            
        }

        private void txt_letra_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void txt_letra_KeyPress(object sender, KeyPressEventArgs e)
        {
            string numeros = "1234567890";
            if(numeros.Contains(e.KeyChar.ToString()) || char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<string> possibilidades = new List<string>();
            listBox2.Items.Clear();
            
            Encoding destEncoding = Encoding.GetEncoding("iso-8859-8");
            txt_encriptado_pm.Text = destEncoding.GetString(Encoding.Convert(Encoding.UTF8, destEncoding, Encoding.UTF8.GetBytes(txt_encriptado_pm.Text)));
           
            string encriptado = txt_encriptado_pm.Text.ToLower();
            int numeroEspacos = -1;
            int numeroDeLetrasAnterior = -1;
            string anterior = "";
            for(int i = 1; i <= 100;i++)
            {
                
            

                    string decifrada = "";

                    string[] array = encriptado.Split();
                    if (numeroEspacos == -1)
                    {
                        numeroEspacos = array.Length;
                    }

                    foreach (string s in array)
                    {
                        for (int j = 0; j < s.Length; j += i + 1)
                        {
                            decifrada += s[j].ToString();
                        }

                        decifrada += " ";
                    }

                
                    if(decifrada != anterior && numeroDeLetrasAnterior != decifrada.Length)
                        possibilidades.Add(decifrada);
                    anterior = decifrada;
                numeroDeLetrasAnterior = decifrada.Length;


            }


            
            
            foreach (string s in possibilidades)
            {
                listBox2.Items.Add(s);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string encriptado = txt_encriptado_metades.Text;
            string desencriptado = "";

            string[] linhas = encriptado.Split(' ');

            try
            {
                if (linhas[0].Length < linhas[1].Length)
                {
                    while (linhas[0].Length != linhas[1].Length)
                    {
                        linhas[0] += " ";
                    }
                }
                else if(linhas[1].Length < linhas[0].Length)
                {
                    while (linhas[0].Length != linhas[1].Length)
                    {
                        linhas[1] += " ";
                    }
                }
                for(int i = 0; i < linhas[0].Length;i++)
                {
                    desencriptado+= linhas[0][i].ToString() + linhas[1][i];
                }

                txt_desencriptado_metades.Text = desencriptado;
            }
            catch
            {
                MessageBox.Show("Formato incorreto");
            }

        }

        private void line1_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string alfabeto = "abcdefghijklmnopqrstuvwxyz";
            char primeroCaracter;
            string cifra = "";
            txt_desencriptado_cmais.Text = "";
            if(numericUpDown1.Value > 0)
            {
                primeroCaracter = alfabeto[Convert.ToInt32((numericUpDown1.Value))];
            }
            else if(numericUpDown1.Value < 0)
            {
                primeroCaracter = alfabeto[alfabeto.Length - 1 + Convert.ToInt32(numericUpDown1.Value)];
            }
            else
            {
                MessageBox.Show("Obtem-se o mesmo resultado");
                return;
            }
            for(int i = alfabeto.IndexOf(primeroCaracter); i < alfabeto.Length;i++)
            {
                cifra += alfabeto[i];
            }
            for(int i = 0; i <= alfabeto.IndexOf(primeroCaracter);i++)
            {
                cifra += alfabeto[i];
            }
            foreach(char c in txt_encriptado_cmais.Text.ToLower())
            {
                if(c == ' ')
                {
                    txt_desencriptado_cmais.Text += " ";
                    continue;
                }
                txt_desencriptado_cmais.Text += alfabeto[cifra.IndexOf(c)];
            }
        }

        
    }
}
