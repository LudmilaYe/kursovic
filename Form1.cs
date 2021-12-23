using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace kursovic
{
    public partial class Form1 : Form
    {
        List<Emitter> emitters = new List<Emitter>(); // Список эмиттеров
        Emitter emitter; // Нужный нам эмиттер

        Point point; // Будущая окружность

        float movement = 0; // Добавили переменную для движения эмиттера по окружности
        public Form1()
        {
            InitializeComponent();

            // Привязка изображения
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            point = new Point // Создаю точку (окружность) по центру формы
            {
                X = picDisplay.Width / 2,
                Y = picDisplay.Height / 2,
            };

            this.emitter = new Emitter // создаю эмиттер и привязываю его к полю emitter
            {
                GravitationY = 0,
                Direction = 0, // Направление движения
                Spreading = 100, // Разброс частицы
                ColorFrom = Color.Gold, // Начальный цвет частицы
                ColorTo = Color.FromArgb(0, Color.Red), // Градация цвета частицы от золотого к красному
                ParticlesPerTick = 10, // Начальное кол-во частиц в тик
                X = (int)point.X, // X координата частицы в пространстве
                Y = (int)point.Y, // Y координата частицы в пространстве
            };

            emitters.Add(emitter); // Обновляем список эмиттеров, добавив в него новый

            emitter.impactPoints.Add(point); // Добавляем специальную точку (окружность) в список специальных точек
        }

        private void timer1_Tick(object sender, EventArgs e)//событие для таймера
        {
            emitter.UpdateState(); // Каждый тик обновляем систему
            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black); // Очистка и окраска фона в черный цвет

                // Так работает движение эмиттера по кругу
                emitter.X = (int)(point.X + point.Radius / 2 * Math.Cos(movement));
                emitter.Y = (int)(point.Y + point.Radius / 2 * Math.Sin(movement));

                emitter.Render(g); // Рендер системы
            }
            emitter.Direction -= (int)(point.Radius / 20); // Вращение эмиттера
            movement += 0.01f * point.Direction; // Увеличиваем переменную движения, чтобы при обновлении системы
                                                 // было ощущение движения эмиттера по кругу 
            picDisplay.Invalidate(); // Обновление picDisplay
        }

        private void tbRadius_Scroll(object sender, EventArgs e)
        {
            point.Radius = tbRadius.Value;
        }

        private void tbSpeed_Scroll(object sender, EventArgs e)
        {
            point.Direction = tbSpeed.Value;
        }

        private void tbCountOfParticlesPerTick_Scroll(object sender, EventArgs e)
        {
            emitter.ParticlesPerTick = tbCountOfParticlesPerTick.Value;
        }
    }

}