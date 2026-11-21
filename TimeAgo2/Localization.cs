using System.Globalization;
using Microsoft.Maui.Storage;

namespace TimeAgo2;

public static class Localization
{
    private const string LanguagePreferenceKey = "app_language";
    private const string DefaultLanguage = "fr-FR";

    private static readonly IReadOnlyDictionary<string, CultureInfo> Cultures =
        new Dictionary<string, CultureInfo>
        {
            ["fr-FR"] = CultureInfo.GetCultureInfo("fr-FR"),
            ["en-US"] = CultureInfo.GetCultureInfo("en-US"),
            ["es-ES"] = CultureInfo.GetCultureInfo("es-ES"),
            ["de-DE"] = CultureInfo.GetCultureInfo("de-DE")
        };

    private static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Text =
        new Dictionary<string, IReadOnlyDictionary<string, string>>
        {
            ["fr-FR"] = new Dictionary<string, string>
            {
                ["main_title"] = "Mes activités",
                ["tagline"] = "MON TEMPS",
                ["main_subtitle"] = "Depuis combien de temps ?",
                ["settings_button"] = "Paramètres",
                ["activity_placeholder"] = "Ex. : dernière séance de sport",
                ["date"] = "DATE",
                ["time"] = "HEURE",
                ["add_activity"] = "Ajouter l’activité",
                ["empty_title"] = "Aucune activité pour le moment",
                ["empty_subtitle"] = "Ajoutez-en une ci-dessus pour commencer.",
                ["delete"] = "Supprimer",
                ["missing_name_title"] = "Nom manquant",
                ["missing_name_message"] = "Saisissez le nom de l’activité.",
                ["invalid_date_title"] = "Date invalide",
                ["invalid_date_message"] = "La date et l’heure ne peuvent pas être dans le futur.",
                ["delete_title"] = "Supprimer l’activité",
                ["delete_message"] = "Voulez-vous supprimer « {0} » ?",
                ["cancel"] = "Annuler",
                ["confirm_delete"] = "Supprimer",
                ["ok"] = "OK",
                ["invalid_json_title"] = "Fichier JSON invalide",
                ["invalid_json_message"] = "Les activités n’ont pas pu être lues : {0}",
                ["read_error_title"] = "Erreur de lecture",
                ["read_error_message"] = "Impossible de lire le fichier des activités : {0}",
                ["access_denied_title"] = "Accès refusé",
                ["read_access_denied_message"] = "Impossible d’accéder au fichier des activités : {0}",
                ["save_access_denied_message"] = "Impossible d’enregistrer les activités : {0}",
                ["save_error_title"] = "Erreur de sauvegarde",
                ["settings_title"] = "Paramètres",
                ["settings_subtitle"] = "Choisissez la langue de l’application.",
                ["language"] = "Langue",
                ["save_settings"] = "Enregistrer",
                ["choose_language_title"] = "Langue non sélectionnée",
                ["choose_language_message"] = "Choisissez une langue avant de continuer.",
                ["just_now"] = "À l’instant",
                ["in_future"] = "À venir",
                ["ago_single"] = "Il y a {0}",
                ["ago_pair"] = "Il y a {0} et {1}",
                ["day_one"] = "jour",
                ["day_many"] = "jours",
                ["hour_one"] = "heure",
                ["hour_many"] = "heures",
                ["minute_one"] = "minute",
                ["minute_many"] = "minutes",
                ["second_one"] = "seconde",
                ["second_many"] = "secondes"
            },
            ["en-US"] = new Dictionary<string, string>
            {
                ["main_title"] = "My activities",
                ["tagline"] = "MY TIME",
                ["main_subtitle"] = "How long has it been?",
                ["settings_button"] = "Settings",
                ["activity_placeholder"] = "e.g. Last workout",
                ["date"] = "DATE",
                ["time"] = "TIME",
                ["add_activity"] = "Add activity",
                ["empty_title"] = "No activities yet",
                ["empty_subtitle"] = "Add one above to get started.",
                ["delete"] = "Delete",
                ["missing_name_title"] = "Name required",
                ["missing_name_message"] = "Enter an activity name.",
                ["invalid_date_title"] = "Invalid date",
                ["invalid_date_message"] = "The date and time cannot be in the future.",
                ["delete_title"] = "Delete activity",
                ["delete_message"] = "Delete “{0}”?",
                ["cancel"] = "Cancel",
                ["confirm_delete"] = "Delete",
                ["ok"] = "OK",
                ["invalid_json_title"] = "Invalid JSON file",
                ["invalid_json_message"] = "Activities could not be read: {0}",
                ["read_error_title"] = "Read error",
                ["read_error_message"] = "Could not read the activities file: {0}",
                ["access_denied_title"] = "Access denied",
                ["read_access_denied_message"] = "Could not access the activities file: {0}",
                ["save_access_denied_message"] = "Could not save activities: {0}",
                ["save_error_title"] = "Save error",
                ["settings_title"] = "Settings",
                ["settings_subtitle"] = "Choose the app language.",
                ["language"] = "Language",
                ["save_settings"] = "Save",
                ["choose_language_title"] = "No language selected",
                ["choose_language_message"] = "Choose a language before continuing.",
                ["just_now"] = "Just now",
                ["in_future"] = "In the future",
                ["ago_single"] = "{0} ago",
                ["ago_pair"] = "{0} and {1} ago",
                ["day_one"] = "day",
                ["day_many"] = "days",
                ["hour_one"] = "hour",
                ["hour_many"] = "hours",
                ["minute_one"] = "minute",
                ["minute_many"] = "minutes",
                ["second_one"] = "second",
                ["second_many"] = "seconds"
            },
            ["es-ES"] = new Dictionary<string, string>
            {
                ["main_title"] = "Mis actividades",
                ["tagline"] = "MI TIEMPO",
                ["main_subtitle"] = "¿Cuánto tiempo ha pasado?",
                ["settings_button"] = "Ajustes",
                ["activity_placeholder"] = "Ej.: último entrenamiento",
                ["date"] = "FECHA",
                ["time"] = "HORA",
                ["add_activity"] = "Añadir actividad",
                ["empty_title"] = "Aún no hay actividades",
                ["empty_subtitle"] = "Añade una arriba para empezar.",
                ["delete"] = "Eliminar",
                ["missing_name_title"] = "Falta el nombre",
                ["missing_name_message"] = "Escribe el nombre de la actividad.",
                ["invalid_date_title"] = "Fecha no válida",
                ["invalid_date_message"] = "La fecha y la hora no pueden estar en el futuro.",
                ["delete_title"] = "Eliminar actividad",
                ["delete_message"] = "¿Quieres eliminar «{0}»?",
                ["cancel"] = "Cancelar",
                ["confirm_delete"] = "Eliminar",
                ["ok"] = "OK",
                ["invalid_json_title"] = "Archivo JSON no válido",
                ["invalid_json_message"] = "No se pudieron leer las actividades: {0}",
                ["read_error_title"] = "Error de lectura",
                ["read_error_message"] = "No se pudo leer el archivo de actividades: {0}",
                ["access_denied_title"] = "Acceso denegado",
                ["read_access_denied_message"] = "No se pudo acceder al archivo de actividades: {0}",
                ["save_access_denied_message"] = "No se pudieron guardar las actividades: {0}",
                ["save_error_title"] = "Error al guardar",
                ["settings_title"] = "Ajustes",
                ["settings_subtitle"] = "Elige el idioma de la aplicación.",
                ["language"] = "Idioma",
                ["save_settings"] = "Guardar",
                ["choose_language_title"] = "No se ha seleccionado un idioma",
                ["choose_language_message"] = "Selecciona un idioma para continuar.",
                ["just_now"] = "Ahora mismo",
                ["in_future"] = "En el futuro",
                ["ago_single"] = "Hace {0}",
                ["ago_pair"] = "Hace {0} y {1}",
                ["day_one"] = "día",
                ["day_many"] = "días",
                ["hour_one"] = "hora",
                ["hour_many"] = "horas",
                ["minute_one"] = "minuto",
                ["minute_many"] = "minutos",
                ["second_one"] = "segundo",
                ["second_many"] = "segundos"
            },
            ["de-DE"] = new Dictionary<string, string>
            {
                ["main_title"] = "Meine Aktivitäten",
                ["tagline"] = "MEINE ZEIT",
                ["main_subtitle"] = "Wie lange ist es her?",
                ["settings_button"] = "Einstellungen",
                ["activity_placeholder"] = "z. B. letztes Training",
                ["date"] = "DATUM",
                ["time"] = "UHRZEIT",
                ["add_activity"] = "Aktivität hinzufügen",
                ["empty_title"] = "Noch keine Aktivitäten",
                ["empty_subtitle"] = "Füge oben eine Aktivität hinzu.",
                ["delete"] = "Löschen",
                ["missing_name_title"] = "Name fehlt",
                ["missing_name_message"] = "Gib einen Namen für die Aktivität ein.",
                ["invalid_date_title"] = "Ungültiges Datum",
                ["invalid_date_message"] = "Datum und Uhrzeit dürfen nicht in der Zukunft liegen.",
                ["delete_title"] = "Aktivität löschen",
                ["delete_message"] = "„{0}“ löschen?",
                ["cancel"] = "Abbrechen",
                ["confirm_delete"] = "Löschen",
                ["ok"] = "OK",
                ["invalid_json_title"] = "Ungültige JSON-Datei",
                ["invalid_json_message"] = "Aktivitäten konnten nicht gelesen werden: {0}",
                ["read_error_title"] = "Lesefehler",
                ["read_error_message"] = "Die Aktivitätsdatei konnte nicht gelesen werden: {0}",
                ["access_denied_title"] = "Zugriff verweigert",
                ["read_access_denied_message"] = "Auf die Aktivitätsdatei konnte nicht zugegriffen werden: {0}",
                ["save_access_denied_message"] = "Aktivitäten konnten nicht gespeichert werden: {0}",
                ["save_error_title"] = "Speicherfehler",
                ["settings_title"] = "Einstellungen",
                ["settings_subtitle"] = "Wähle die Sprache der App.",
                ["language"] = "Sprache",
                ["save_settings"] = "Speichern",
                ["choose_language_title"] = "Keine Sprache ausgewählt",
                ["choose_language_message"] = "Wähle eine Sprache aus, bevor du fortfährst.",
                ["just_now"] = "Gerade eben",
                ["in_future"] = "In der Zukunft",
                ["ago_single"] = "Vor {0}",
                ["ago_pair"] = "Vor {0} und {1}",
                ["day_one"] = "einem Tag",
                ["day_many"] = "Tagen",
                ["hour_one"] = "einer Stunde",
                ["hour_many"] = "Stunden",
                ["minute_one"] = "einer Minute",
                ["minute_many"] = "Minuten",
                ["second_one"] = "einer Sekunde",
                ["second_many"] = "Sekunden"
            }
        };

    public static IReadOnlyList<(string Code, string Name)> Languages { get; } =
    [
        ("fr-FR", "Français"),
        ("en-US", "English"),
        ("es-ES", "Español"),
        ("de-DE", "Deutsch")
    ];

    public static string CurrentLanguage { get; private set; } = DefaultLanguage;

    public static CultureInfo CurrentCulture => Cultures[CurrentLanguage];

    public static void Initialize()
    {
        var savedLanguage = Preferences.Default.Get(LanguagePreferenceKey, DefaultLanguage);
        ApplyCulture(Cultures.ContainsKey(savedLanguage) ? savedLanguage : DefaultLanguage);
    }

    public static void SetLanguage(string languageCode)
    {
        if (!Cultures.ContainsKey(languageCode))
        {
            throw new ArgumentOutOfRangeException(nameof(languageCode), languageCode, "Unsupported language.");
        }

        Preferences.Default.Set(LanguagePreferenceKey, languageCode);
        ApplyCulture(languageCode);
    }

    public static string Get(string key) => Text[CurrentLanguage][key];

    public static string Format(string key, params object[] arguments) =>
        string.Format(CurrentCulture, Get(key), arguments);

    public static string FormatElapsed(DateTimeOffset createdAt, DateTimeOffset now)
    {
        var elapsed = now - createdAt;
        if (elapsed < TimeSpan.Zero)
        {
            return Get("in_future");
        }

        if (elapsed.TotalSeconds < 1)
        {
            return Get("just_now");
        }

        if (elapsed.TotalDays >= 1)
        {
            var days = Quantity(elapsed.Days, "day");
            return elapsed.Hours > 0
                ? Format("ago_pair", days, Quantity(elapsed.Hours, "hour"))
                : Format("ago_single", days);
        }

        if (elapsed.TotalHours >= 1)
        {
            return Format(
                "ago_pair",
                Quantity(elapsed.Hours, "hour"),
                Quantity(elapsed.Minutes, "minute"));
        }

        if (elapsed.TotalMinutes >= 1)
        {
            return Format("ago_single", Quantity(elapsed.Minutes, "minute"));
        }

        return Format("ago_single", Quantity(elapsed.Seconds, "second"));
    }

    private static string Quantity(int count, string unit)
    {
        var singular = count == 1;
        var unitText = Get($"{unit}_{(singular ? "one" : "many")}");
        return CurrentLanguage == "de-DE" && singular ? unitText : $"{count} {unitText}";
    }

    private static void ApplyCulture(string languageCode)
    {
        CurrentLanguage = languageCode;
        var culture = Cultures[languageCode];
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }
}
