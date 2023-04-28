using System;

namespace WpfEscapeGame
{
    public enum MessageType
    {
        Info,
        Pickup,
        Error
    }

    internal static class RandomMessageGenerator
    {
        private static string[] msgInfo = new[]
        {
            "I think I'm on the right track.",
            "Ohh, this looks interesting.",
            "Let's continue exploring."
        };

        private static string[] msgPickup = new[]
        {
            "How beautiful!.",
            "Let's go! New piece for my collection.",
            "Interesting looking item."
        };

        private static string[] msgError = new[]
        {
            "Oops, I think something is wrong.",
            "This is not good, I have to find another way.",
            "That was certainly not the intention."
        };

        private static Random random = new Random();

        public static string GetRandomMessage(MessageType type)
        {
            switch (type)
            {
                case MessageType.Info:
                    return msgInfo[random.Next(msgInfo.Length)];
                case MessageType.Pickup:
                    return msgPickup[random.Next(msgPickup.Length)];
                case MessageType.Error:
                    return msgError[random.Next(msgError.Length)];
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
