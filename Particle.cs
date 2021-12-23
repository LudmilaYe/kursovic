using System;
using System.Drawing;

namespace kursovic
{
    public class Particle
    {
        public int radius; // Обозначаем радиус частиц
        public float X; // X координата положения частицы в пространстве
        public float Y; // Y координата положения частицы в пространстве

        public float speedX; // Скорость по X
        public float speedY; // Скорость по Y

        public float life; // Кол-во здоровья частиц

        public static Random random = new Random(); // Генератор случайных чисел

        public Particle() // конструктор по умолчанию будет создавать кастомную частицу
        {
            var direction = (double)random.Next(360); // Направление движения
            var speed = 1 + random.Next(10); // 

            speedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed); // Скорость по X
            speedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed); // Скорость по Y

            radius = 2 + random.Next(10); // Радиус частицы
            life = 20 + random.Next(100); // Кол-во 'здоровья' частицы
        }

        public virtual void Draw(Graphics g)
        {
            float k = Math.Min(1f, life / 100); // Коэффициент прозрачности от 0 до 1.0
            int alpha = (int)(k * 255); // рассчитываем значение альфа канала в шкале от 0 до 255

            Color color = Color.FromArgb(alpha, Color.Violet); //Добавили цвет для кисти

            var b = new SolidBrush(color); // создали кисть для рисования
            // нарисовали залитый кружок радиусом Radius с центром в X, Y
            g.FillEllipse(b, X - radius, Y - radius, radius * 2, radius * 2);

            // удалили кисть из памяти, вообще сборщик мусора рано или поздно это сам сделает
            // но документация рекомендует делать это самому
            b.Dispose();
        }
    }

    public class ParticleColorful : Particle
    {
        public Color FromColor; // Начальный цвет
        public Color ToColor; // Конечный цвет 

        public static Color MixColor(Color color1, Color color2, float k) // Задаем цвет от начального к конечному
        {
            return Color.FromArgb(
                (int)(color2.A * k + color1.A * (1 - k)),
                (int)(color2.R * k + color1.R * (1 - k)),
                (int)(color2.G * k + color1.G * (1 - k)),
                (int)(color2.B * k + color1.B * (1 - k))
                );
        }

        public override void Draw(Graphics g) // Отрисовка частиц
        {
            float k = Math.Min(1f, life / 100); // Задаем коэффициент прозрачности в зависимости от кол-ва здоровья частицы

            var color = MixColor(ToColor, FromColor, k); // Добавляем цвет для кисти
            var b = new SolidBrush(color); // создали кисть для рисования
            // нарисовали залитый кружок радиусом Radius с центром в X, Y
            g.FillEllipse(b, X - radius, Y - radius, radius * 2, radius * 2);

            // удалили кисть из памяти, вообще сборщик мусора рано или поздно это сам сделает
            // но документация рекомендует делать это самому
            b.Dispose();
        }
    }
}
