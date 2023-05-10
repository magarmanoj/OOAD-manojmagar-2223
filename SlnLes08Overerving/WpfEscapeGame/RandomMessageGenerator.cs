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
            "I think this is locked",
            "Ohh, this looks interestingn but it's locked.",
            "It's locked."
        };

        private static string[] isWrongKeyMsgs = new[]
        {
            "Wrong key.",
            "You are using the wrong key.",
            "This key won't unlock it."
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
