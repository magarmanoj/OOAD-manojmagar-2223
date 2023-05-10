using System;

namespace WpfEscapeGame
{
    public enum MessageType
    {
        IS_LOCKED,
        IS_WRONG_KEY,
        DOES_NOT_WORK
    }

    public static class RandomMessageGenerator
    {
        private static string[] isLockedMsgs = new[]
        {
            "I think I'm on the right track.",
            "Ohh, this looks interesting.",
            "Let's continue exploring."
        };

        private static string[] isWrongKeyMsgs = new[]
        {
            "How beautiful!.",
            "Let's go! New piece for my collection.",
            "Interesting looking item."
        };

        private static string[] doesNotWorkMsgs = new[]
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
                case MessageType.IS_LOCKED:
                    return isLockedMsgs[random.Next(isLockedMsgs.Length)];
                case MessageType.IS_WRONG_KEY:
                    return isWrongKeyMsgs[random.Next(isWrongKeyMsgs.Length)];
                case MessageType.DOES_NOT_WORK:
                    return doesNotWorkMsgs[random.Next(doesNotWorkMsgs.Length)];
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
