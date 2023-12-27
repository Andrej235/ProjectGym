namespace AppProjectGym.Utilities.Models
{
    public class Vector2(float x, float y)
    {
        public float X { get; private set; } = x;
        public float Y { get; private set; } = y;

        public void Normalize()
        {
            float length = (float)Math.Sqrt(X * X + Y * Y);

            if (length != 0)
            {
                X /= length;
                Y /= length;
            }
        }

        public void Rotate(float degrees)
        {
            float radians = (float)(degrees * Math.PI / 180.0);
            float newX = X * (float)Math.Cos(radians) - Y * (float)Math.Sin(radians);
            float newY = X * (float)Math.Sin(radians) + Y * (float)Math.Cos(radians);

            X = newX;
            Y = newY;
        }

        public static float GetRotationAngle(Vector2 vector1, Vector2 vector2)
        {
            float crossProduct = vector1.X * vector2.Y - vector1.Y * vector2.X;
            float dotProduct = vector1.X * vector2.X + vector1.Y * vector2.Y;

            float angleRadians = (float)Math.Atan2(crossProduct, dotProduct);
            float angleDegrees = (float)(angleRadians * 180.0 / Math.PI);

            return angleDegrees;
        }
        public float GetRotationAngle(Vector2 vector2) => GetRotationAngle(this, vector2);
        public float GetRotationAngle()
        {
            var vector2 = new Vector2(0, -1);
            vector2.Normalize();

            return GetRotationAngle(this, vector2);
        }

        public float Magnitude => (float)Math.Sqrt(X * X + Y * Y);

        public override string ToString() => $"({X}, {Y})";
    }
}
