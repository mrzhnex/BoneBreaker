namespace BoneBreaker
{
    class Global
    {
        //rework study
        public static int hit_damage = 10;
        public static float distance_to_hit = 2f;
        public static string _starthit = "Вы замахнулись...";
        public static string _notlook = "Вы не смотрите на цель, либо находитесь далеко/слишком близко от нее";

        public static string _cooldown = "Подождите еще секунд: ";
        public static string _successhit1 = "Вы ударили ";
        public static string _successhit2 = " и нанесли урона: ";
        public static float cooldown = 5f;

        public static string _targetscp = "Цель - SCP, вы не нанесли ощутимого вреда";
        public static string _iscp = "Вы не можете совершить это действие, так как вы - SCP";
        internal static bool can_use_commands;

        public static bool IsFullRp = false;
    }
}