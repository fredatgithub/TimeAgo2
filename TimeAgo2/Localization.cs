using System.Globalization;

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
          ["main_title"] = "Événements",
          ["tagline"] = "MES ÉVÉNEMENTS",
          ["main_subtitle"] = "Quand avez-vous fait cela pour la dernière fois ?",
          ["settings_button"] = "Paramètres",
          ["activity_placeholder"] = "Nom de l’événement",
          ["date"] = "DATE",
          ["time"] = "HEURE",
          ["add_activity"] = "Créer l’événement",
          ["add_again"] = "Ajouter une occurrence",
          ["add_event"] = "Ajouter un événement",
          ["create_event"] = "Créer l’événement",
          ["add_instance"] = "Ajouter une occurrence",
          ["instance_note_placeholder"] = "Note pour cette occurrence (facultative)",
          ["instance_default_title"] = "Occurrence",
          ["save_instance"] = "Ajouter l’occurrence",
          ["instance_history"] = "Historique des occurrences",
          ["timeline_entry"] = "{0} – {1}",
          ["empty_title"] = "Aucun événement pour le moment",
          ["empty_subtitle"] = "Ajoutez un événement pour commencer.",
          ["empty_instances"] = "Aucune occurrence",
          ["back"] = "Retour",
          ["delete_event"] = "Supprimer l’événement",
          ["delete_event_title"] = "Supprimer l’événement",
          ["delete_event_message"] = "Supprimer « {0} » et tout son historique ?",
          ["delete_instance_title"] = "Supprimer l’occurrence",
          ["delete_instance_message"] = "Supprimer cette occurrence de « {0} » ?",
          ["delete_last_instance_message"] = "C’est la dernière occurrence. Supprimer l’événement « {0} » ?",
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
          ["theme"] = "Thème",
          ["theme_light"] = "Clair",
          ["theme_dark"] = "Sombre",
          ["save_settings"] = "Enregistrer",
          ["about"] = "À propos",
          ["about_title"] = "À propos de TimeAgo2",
          ["about_subtitle"] = "Suivez le temps écoulé depuis vos activités.",
          ["about_description"] = "TimeAgo2 vous permet de créer des événements, de suivre leurs occurrences et de consulter l’historique de chacune.",
          ["about_version"] = "Version",
          ["about_author"] = "Auteur",
          ["about_contact"] = "Adresse e-mail de contact",
          ["about_website"] = "Site web",
          ["back_to_settings"] = "Retour aux paramètres",
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
          ["main_title"] = "Events",
          ["tagline"] = "MY EVENTS",
          ["main_subtitle"] = "When did you last do this?",
          ["settings_button"] = "Settings",
          ["activity_placeholder"] = "Event name",
          ["date"] = "DATE",
          ["time"] = "TIME",
          ["add_activity"] = "Create event",
          ["add_again"] = "Add instance",
          ["add_event"] = "Add event",
          ["create_event"] = "Create event",
          ["add_instance"] = "Add instance",
          ["instance_note_placeholder"] = "Instance note (optional)",
          ["instance_default_title"] = "Instance",
          ["save_instance"] = "Add instance",
          ["instance_history"] = "Instance history",
          ["timeline_entry"] = "{0} - {1}",
          ["empty_title"] = "No events yet",
          ["empty_subtitle"] = "Add an event to get started.",
          ["empty_instances"] = "No instances",
          ["back"] = "Back",
          ["delete_event"] = "Delete event",
          ["delete_event_title"] = "Delete event",
          ["delete_event_message"] = "Delete “{0}” and its entire history?",
          ["delete_instance_title"] = "Delete instance",
          ["delete_instance_message"] = "Delete this instance of “{0}”?",
          ["delete_last_instance_message"] = "This is the last instance. Delete event “{0}”?",
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
          ["theme"] = "Theme",
          ["theme_light"] = "Light",
          ["theme_dark"] = "Dark",
          ["save_settings"] = "Save",
          ["about"] = "About",
          ["about_title"] = "About TimeAgo2",
          ["about_subtitle"] = "Track the time elapsed since your activities.",
          ["about_description"] = "TimeAgo2 lets you create events, track their occurrences and review the history of each one.",
          ["about_version"] = "Version",
          ["about_author"] = "Author",
          ["about_contact"] = "Contact email",
          ["about_website"] = "Website",
          ["back_to_settings"] = "Back to settings",
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
          ["main_title"] = "Eventos",
          ["tagline"] = "MIS EVENTOS",
          ["main_subtitle"] = "¿Cuándo fue la última vez que hiciste esto?",
          ["settings_button"] = "Ajustes",
          ["activity_placeholder"] = "Nombre del evento",
          ["date"] = "FECHA",
          ["time"] = "HORA",
          ["add_activity"] = "Crear evento",
          ["add_again"] = "Añadir ocurrencia",
          ["add_event"] = "Añadir evento",
          ["create_event"] = "Crear evento",
          ["add_instance"] = "Añadir ocurrencia",
          ["instance_note_placeholder"] = "Nota de esta ocurrencia (opcional)",
          ["instance_default_title"] = "Ocurrencia",
          ["save_instance"] = "Añadir ocurrencia",
          ["instance_history"] = "Historial de ocurrencias",
          ["timeline_entry"] = "{0} - {1}",
          ["empty_title"] = "Aún no hay eventos",
          ["empty_subtitle"] = "Añade un evento para empezar.",
          ["empty_instances"] = "No hay ocurrencias",
          ["back"] = "Volver",
          ["delete_event"] = "Eliminar evento",
          ["delete_event_title"] = "Eliminar evento",
          ["delete_event_message"] = "¿Eliminar «{0}» y todo su historial?",
          ["delete_instance_title"] = "Eliminar ocurrencia",
          ["delete_instance_message"] = "¿Eliminar esta ocurrencia de «{0}»?",
          ["delete_last_instance_message"] = "Es la última ocurrencia. ¿Eliminar el evento «{0}»?",
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
          ["theme"] = "Tema",
          ["theme_light"] = "Claro",
          ["theme_dark"] = "Oscuro",
          ["save_settings"] = "Guardar",
          ["about"] = "Acerca de",
          ["about_title"] = "Acerca de TimeAgo2",
          ["about_subtitle"] = "Sigue el tiempo transcurrido desde tus actividades.",
          ["about_description"] = "TimeAgo2 permite crear eventos, registrar sus ocurrencias y consultar el historial de cada uno.",
          ["about_version"] = "Versión",
          ["about_author"] = "Autor",
          ["about_contact"] = "Correo electrónico de contacto",
          ["about_website"] = "Sitio web",
          ["back_to_settings"] = "Volver a ajustes",
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
          ["main_title"] = "Ereignisse",
          ["tagline"] = "MEINE EREIGNISSE",
          ["main_subtitle"] = "Wann hast du das zuletzt gemacht?",
          ["settings_button"] = "Einstellungen",
          ["activity_placeholder"] = "Name des Ereignisses",
          ["date"] = "DATUM",
          ["time"] = "UHRZEIT",
          ["add_activity"] = "Ereignis erstellen",
          ["add_again"] = "Vorkommen hinzufügen",
          ["add_event"] = "Ereignis hinzufügen",
          ["create_event"] = "Ereignis erstellen",
          ["add_instance"] = "Vorkommen hinzufügen",
          ["instance_note_placeholder"] = "Notiz zu diesem Vorkommen (optional)",
          ["instance_default_title"] = "Vorkommen",
          ["save_instance"] = "Vorkommen hinzufügen",
          ["instance_history"] = "Verlauf der Vorkommen",
          ["timeline_entry"] = "{0} - {1}",
          ["empty_title"] = "Noch keine Ereignisse",
          ["empty_subtitle"] = "Füge ein Ereignis hinzu, um loszulegen.",
          ["empty_instances"] = "Keine Vorkommen",
          ["back"] = "Zurück",
          ["delete_event"] = "Ereignis löschen",
          ["delete_event_title"] = "Ereignis löschen",
          ["delete_event_message"] = "„{0}“ und den gesamten Verlauf löschen?",
          ["delete_instance_title"] = "Vorkommen löschen",
          ["delete_instance_message"] = "Dieses Vorkommen von „{0}“ löschen?",
          ["delete_last_instance_message"] = "Dies ist das letzte Vorkommen. Ereignis „{0}“ löschen?",
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
          ["theme"] = "Design",
          ["theme_light"] = "Hell",
          ["theme_dark"] = "Dunkel",
          ["save_settings"] = "Speichern",
          ["about"] = "Über",
          ["about_title"] = "Über TimeAgo2",
          ["about_subtitle"] = "Verfolge die verstrichene Zeit seit deinen Aktivitäten.",
          ["about_description"] = "Mit TimeAgo2 kannst du Ereignisse erstellen, ihre Vorkommen erfassen und den Verlauf jedes Ereignisses ansehen.",
          ["about_version"] = "Version",
          ["about_author"] = "Autor",
          ["about_contact"] = "Kontakt-E-Mail",
          ["about_website"] = "Webseite",
          ["back_to_settings"] = "Zurück zu den Einstellungen",
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
