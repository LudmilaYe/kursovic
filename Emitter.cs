using System;
using System.Collections.Generic;
using System.Drawing;

namespace kursovic
{
    public class Emitter
    {
        public List<Particle> particles = new List<Particle>(); // Список частиц
        public List<IImpactPoint> impactPoints = new List<IImpactPoint>(); // Список специальных точек

        public float GravitationX = 0; // Гравитация по X
        public float GravitationY = 1; // Гравитация по Y

        public int X; // координата X центра эмиттера, будем ее использовать вместо MousePositionX
        public int Y; // соответствующая координата Y 
        public int Direction = 0; // вектор направления в градусах куда сыпет эмиттер
        public int Spreading = 360; // разброс частиц относительно Direction
        public int SpeedMin = 1; // начальная минимальная скорость движения частицы
        public int SpeedMax = 10; // начальная максимальная скорость движения частицы
        public int RadiusMin = 2; // минимальный радиус частицы
        public int RadiusMax = 10; // максимальный радиус частицы
        public int LifeMin = 10; // минимальное время жизни частицы
        public int LifeMax = 100; // максимальное время жизни частицы
        public int ParticlesPerTick = 1; // Кол-во частиц за тик
        public int ParticlesCount = 0; // Kол-во частиц

        public Color ColorFrom = Color.Pink; // начальный цвет частицы
        public Color ColorTo = Color.FromArgb(0, Color.RosyBrown); // конечный цвет частиц

        public void UpdateState() // Метод обновления состояния системы
        {
            int particlesToCreate = ParticlesPerTick; // Добавляю генерацию частиц не больше частиц за тик
            ParticlesCount = 0;
            foreach (var particle in particles)
            {
                if (particle.life <= 0) // если здоровье кончилось
                {
                    ParticlesCount--; // Уменьшаю кол-во активных частиц

                    if (particlesToCreate > 0)
                    {
                        /* у нас как сброс частицы равносилен созданию частицы */
                        particlesToCreate -= 1; // поэтому уменьшаем счётчик созданных частиц на 1
                        ResetParticle(particle); // Вызываю метод респавна частиц (сброс частицы)
                    }
                }
                else
                {
                    particle.X += particle.speedX; // Передвигаю частицу по X 
                    particle.Y += particle.speedY; // Передвигаю частицу по Y

                    particle.life -= 1; // Уменьшаю кол-во здоровья на 1
                    foreach (var point in impactPoints)
                    {
                        point.ImpactParticle(particle);
                    }

                    particle.speedX += GravitationX; // Гравитация по X
                    particle.speedY += GravitationY; // Гравитация по Y
                }
            }

            while (particlesToCreate >= 1) // Пока кол-во 
            {
                particlesToCreate -= 1; // Уменьшаю кол-во частиц 
                var particle = CreateParticle(); // Создаю частицу 
                ResetParticle(particle); // Сбрасываю частицу
                particles.Add(particle); // Добавляю частицу в список
            }
        }

        public virtual void ResetParticle(Particle particle) // Метод сброса частицы
        {
            particle.life = Particle.random.Next(LifeMin, LifeMax); // Задаю кол-во здоровья 
            particle.X = X; // Устанавливаю место генерации частицы по X
            particle.Y = Y; // Устанавливаю место генерации частицы по Y

            var direction = Direction + (double)Particle.random.Next(Spreading) - (Spreading / 2); // Направление движения
            var speed = Particle.random.Next(SpeedMin, SpeedMax); // Скорость частиц

            particle.speedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed); // Скорость частиц по X
            particle.speedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed); // Скорость частиц по Y

            particle.radius = Particle.random.Next(RadiusMin, RadiusMax); // Задаю радиус
        }
        public virtual Particle CreateParticle() // Метод создания частицы
        {
            var particle = new ParticleColorful();
            particle.FromColor = ColorFrom;
            particle.ToColor = ColorTo;

            return particle;
        }

        public void Render(Graphics g) // Отрисовка или рендеринг
        {
            foreach (var particle in particles) // Отрисовка частиц
            {
                particle.Draw(g);
            }
            foreach (var point in impactPoints)
            {
                point.Render(g); // Рендер частиц
            }
        }
    }
}