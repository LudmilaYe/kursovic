using System;
using System.Drawing;

namespace CourseWork_L.Egupova
{
    public abstract class IImpactPoint
    {
        public float X; // ну точка же, вот и две координаты
        public float Y;

        // абстрактный метод с помощью которого будем изменять состояние частиц
        // например притягивать или отталкивать
        public abstract void ImpactParticle(Particle particle);

        // базовый класс для отрисовки точечки
        public virtual void Render(Graphics g)
        {
            g.FillEllipse(
                    new SolidBrush(Color.Red),
                    X - 5,
                    Y - 5,
                    10,
                    10
                );
        }
    }

    public class Point : IImpactPoint
    {
        public int Radius = 100; // Радиус точки
        public int Power = 100; // Сила отторжения
        public int Direction = 10; // Направление движения
        public override void ImpactParticle(Particle particle) // Реализуем метод для изменения состояния частиц
        {
            float gX = X - particle.X;
            float gY = Y - particle.Y;
            float r2 = (float)Math.Max(100, gX * gX + gY * gY);

            particle.speedX -= gX * Power / r2; // Скорость отторжения по Х
            particle.speedY -= gY * Power / r2; // Скорость отторжения по Y
        }
        public override void Render(Graphics g) // Переопределение метода рендеринга
        {
            g.DrawEllipse( // Отрисовываем окружность в центре формы
                   new Pen(Color.Gold),
                   X - Radius / 2,
                   Y - Radius / 2,
                   Radius,
                   Radius
               );
        }
    }
}
