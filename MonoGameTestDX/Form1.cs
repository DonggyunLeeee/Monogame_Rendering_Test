using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HalconDotNet;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace Test_Layer_Points
{
    public partial class Main_Form : Form
    {
        public HWindow mMainWindow;
        public Main_Form()
        {
            InitializeComponent();

            mMainWindow = H_WindowMain.HalconWindow;

        }

        private void but_Test_Click(object sender, EventArgs e)
        {
            string root = "E:\\Test_Layer_Points\\layer_points";


            //Layer_Sub layer_sub = new Layer_Sub();
            //BinaryFormatter bf = new BinaryFormatter();
            //bf.Binder = new CustomizedBinder();
            //FileStream fs = new FileStream(Path.Combine(root, "m_Layer_Sub.bin"), FileMode.Open);
            //layer_sub = (Layer_Sub)bf.Deserialize(fs);
            //fs.Close();


            HTuple hv_Width = 8100;
            HTuple hv_Height = 7000;
            HOperatorSet.GenImageConst(out HObject ho_Image_Sub, "byte", hv_Width, hv_Height);


            Layer_Points layer_points = new Layer_Points();
            layer_points.Read(root);

            for (int i = 0; i < layer_points.point_index.Count; i++)
            {
                int start_pos = layer_points.point_index[i].pos_start;
                int count = layer_points.point_index[i].count;

                float[] arr_X = new float[count];
                float[] arr_Y = new float[count];

                Array.Copy(layer_points.point_X, start_pos, arr_X, 0, count);
                Array.Copy(layer_points.point_Y, start_pos, arr_Y, 0, count);

                HTuple row = new HTuple(arr_Y);
                HTuple col = new HTuple(arr_X);

                HOperatorSet.GenContourPolygonXld(out HObject contour, row, col);

                HTuple gray = (layer_points.point_index[i].polarity == 'P') ? 255 : 0;

                HOperatorSet.PaintXld(contour, ho_Image_Sub, out ho_Image_Sub, gray);

                int test = 0;
                if (test > 0)
                {
                    HOperatorSet.WriteObject(ho_Image_Sub, root + "\\ho_Image_Sub" + test);
                    HOperatorSet.WriteObject(contour, root + "\\ho_Contur" + test);
                    test += 1;
                }


            }

            //HOperatorSet.WriteObject(ho_Image_Sub, "C:\\WORK_CURR\\HI-Resolution\\TMP\\ho_Image_Sub");
            HOperatorSet.WriteImage(ho_Image_Sub, "tiff", 0, Path.Combine(root, "ho_Image_Sub"));

            mMainWindow.ClearWindow();
            ho_Image_Sub.DispObj(mMainWindow);
        }
    }
}
