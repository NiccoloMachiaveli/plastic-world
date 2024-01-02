using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectorAI : CoreAI {

    // Состояние, в которых может находиться Protector
    enum Statetype { SAFE, CAUTIOUS, DANGER };

    // Устанавливается из редактора
    public AudioClip dangerClip;    // Фоновая музыка, когда Wanderer в опасности
    public AudioClip[] grunts;      // Случайные звуки, которые Protector издает когда злытся

    Statetype state;            // Текущее состояние Protector
    GameObject wanderer;        // Wanderer связанный с этим Protector
    WandererAI my_wanderer;     // Скрипт, управляющий Wanderer - содержит нужные переменные
    AudioSource source;         // Аудио дорожка связанная с Protector
    float gruntCooldown;        // Добавляет время между созданием звуковых эффектов
    float CAUTION_RADIUS = 8f;  // Расстояние до нападения Protector на игрока


    // Как вести себя, если игрок далеко и не атакует
    void handleSafe()
    {
        // Посмотрите и следуйте за странником
        lookAt(wanderer);
        moveTo(wanderer);

        // переходные состояния, если игрок подходит слишком близко
        if (Vector3.Distance(player_transform.position, wanderer.transform.position) < CAUTION_RADIUS)
            state = Statetype.CAUTIOUS;
    }

    // Как вести себя, если игрок слишком близко
    void handleCautious()
    {
        // Посмотрите и заблокируйте игрока с помощью тела
        lookAt(player);
        moveTo(blockOffPlayer());

        // Иногда издают звук ворчание
        makeGrunt(0.005f);

        // Переход обратно в безопасное состояние, если игрок достаточно далеко
        if (Vector3.Distance(player_transform.position, wanderer.transform.position) > CAUTION_RADIUS)
            state = Statetype.SAFE;
    }

    // Как вести себя, если игрок атакует
    void handleDanger()
    {
        // Обязательно столкнетесь с игроком и не остановитесь на короткой
        my_nav.stoppingDistance = 0f;

        // Проверьте, не подвергается опасности, и игрок достаточно далеко
        if (!my_wanderer.inDanger && distToPlayer() > CAUTION_RADIUS)
        {
            state = Statetype.CAUTIOUS;
            if (source.isPlaying) source.Stop();
            my_nav.stoppingDistance = 0.5f;
        }

        // В противном случае преследовать игрока и играть в опасность музыки
        else
        {
            if (source.clip != dangerClip) source.clip = dangerClip;
            if (!source.isPlaying) source.Play();
            lookAt(player);
            moveTo(player);
            makeGrunt(0.01f);
        }
    }

    // ***** Helper Methods for FSM *****

    // вспомогательный метод определения назначения, если игрок слишком близко
    Vector3 blockOffPlayer()
    {
        Vector3 dir = wanderer.transform.position - player_transform.position;
        Vector3 location = -4 * dir.normalized + wanderer.transform.position;
        return location;
    }

    // вспомогательный метод, который, с некоторой шансом включает звук ворчание
    void makeGrunt(float chance)
    {
        if (gruntCooldown <= 0f && Random.Range(0f, 1f) < chance)
        {
            source.PlayOneShot(grunts[Random.Range(0, grunts.Length)]);
            gruntCooldown = 3f; // 3 секунды в реальном времени до того, как можно сыграть следующее звук ворчание
        }
    }

    // *************** Built-In Methods ****************

	// Use this for initialization
	protected void Start () {
        CoreAIStart();
        state = Statetype.SAFE;
        wanderer = GameObject.FindWithTag("Wanderer");
        my_wanderer = wanderer.GetComponent<WandererAI>();
        source = GetComponent<AudioSource>();
        gruntCooldown = 0f;
    }
	
	// Update is called once per frame
	void Update () {

        // Независимо от состояния, введите состояние опасности, если Странник находится в опасности
        if (my_wanderer.inDanger) state = Statetype.DANGER;

        // Вызвать для обработки текущего состояния
        switch(state)
        {
            case Statetype.SAFE:
                handleSafe();
                break;

            case Statetype.CAUTIOUS:
                handleCautious();
                break;

            case Statetype.DANGER:
                handleDanger();
                break;

            default:
                handleSafe();
                break;
        }

        // отсечь таймер для звуковых эффектов
        if (gruntCooldown >= 0f) gruntCooldown -= Time.deltaTime;
	}
}
