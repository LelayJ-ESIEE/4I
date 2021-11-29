package com.neves.topquiz.controller;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AppCompatActivity;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.res.Configuration;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.neves.topquiz.R;
import com.neves.topquiz.model.Question;
import com.neves.topquiz.model.QuestionBank;
import com.neves.topquiz.model.User;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class GameActivity extends AppCompatActivity implements View.OnClickListener {
    public static final String BUNDLE_STATE_SCORE = "BUNDLE_STATE_SCORE";
    public static final String BUNDLE_STATE_QUESTION = "BUNDLE_STATE_QUESTION";
    public static final String BUNDLE_QUESTION_BANK = "BUNDLE_QUESTION_BANK";
    public static final String MY_FILE_NAME = "questions.txt";

    public static final String USER = "USER";

    private QuestionBank mQuestionBank;
    private Question mCurrentQuestion;

    private TextView mQuestionTextView;
    private Button mAnswerButton1;
    private Button mAnswerButton2;
    private Button mAnswerButton3;
    private Button mAnswerButton4;

    private int mNumberOfQuestions;
    private boolean mEnableTouchEvents;
    private User mUser;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_game);

        System.out.println("GameActivity::onCreate()");

        Intent intent = getIntent();
        if (intent.hasExtra(USER)) {
            mUser = intent.getParcelableExtra(USER);
        }

        deleteFilePerso();
        try {
            Question q = new Question(getString(R.string.question1), Arrays.asList(getString(R.string.response11),
                    getString(R.string.response12), getString(R.string.response13), getString(R.string.response14)), 3);
            saveQuestion(q);
            q = new Question(getString(R.string.question2), Arrays.asList(getString(R.string.response21),
                    getString(R.string.response22), getString(R.string.response23), getString(R.string.response24)), 3);
            saveQuestion(q);
            q = new Question(getString(R.string.question3), Arrays.asList(getString(R.string.response31),
                    getString(R.string.response32), getString(R.string.response33), getString(R.string.response34)), 2);
            saveQuestion(q);
            q = new Question("Question 4", Arrays.asList("cinq", "six", "sept", "bonne reponse"), 3);
            saveQuestion(q);
            q = new Question("Question 5", Arrays.asList("cinq", "six", "sept", "bonne reponse"), 3);
            saveQuestion(q);
            q = new Question("Question 6", Arrays.asList("cinq", "six", "sept", "bonne reponse"), 3);
            saveQuestion(q);
            q = new Question("Question 7", Arrays.asList("cinq", "six", "sept", "bonne reponse"), 3);
            saveQuestion(q);
        } catch (IOException e) {
            e.printStackTrace();
        }

        if (savedInstanceState != null) {
            mUser.setScore(savedInstanceState.getInt(BUNDLE_STATE_SCORE));
            mNumberOfQuestions = savedInstanceState.getInt(BUNDLE_STATE_QUESTION);
            mQuestionBank = savedInstanceState.getParcelable(BUNDLE_QUESTION_BANK);
            mQuestionBank.setmNextQuestionIndex(4 - mNumberOfQuestions);
        } else {
            mUser.setScore(0);
            mNumberOfQuestions = 4;
            try {
                mQuestionBank = this.generateQuestions();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        mEnableTouchEvents = true;

        mQuestionTextView = (TextView) findViewById(R.id.activity_game_question_text);
        mAnswerButton1 = (Button) findViewById(R.id.activity_game_answer1_btn);
        mAnswerButton2 = (Button) findViewById(R.id.activity_game_answer2_btn);
        mAnswerButton3 = (Button) findViewById(R.id.activity_game_answer3_btn);
        mAnswerButton4 = (Button) findViewById(R.id.activity_game_answer4_btn);

        mAnswerButton1.setOnClickListener(this);
        mAnswerButton2.setOnClickListener(this);
        mAnswerButton3.setOnClickListener(this);
        mAnswerButton4.setOnClickListener(this);

        mAnswerButton1.setTag(0);
        mAnswerButton2.setTag(1);
        mAnswerButton3.setTag(2);
        mAnswerButton4.setTag(3);

        mCurrentQuestion = mQuestionBank.getQuestion();
        this.displayQuestion(mCurrentQuestion);
    }

    /**
     * Affiche la question et les réponses dans l'interface
     *
     * @param question : object question
     */
    private void displayQuestion(final Question question) {
        mQuestionTextView.setText(question.getQuestion());
        mAnswerButton1.setText(question.getChoiceList().get(0));
        mAnswerButton2.setText(question.getChoiceList().get(1));
        mAnswerButton3.setText(question.getChoiceList().get(2));
        mAnswerButton4.setText(question.getChoiceList().get(3));
    }

    private void deleteFilePerso(){
        deleteFile(MY_FILE_NAME);
    }

    private void saveQuestion(Question q) throws IOException {

        Log.d("test0", "Question sauvegardée : " + q.getQuestion());
        Log.d("test0", "réponse 1 sauvegardée : " + q.getChoiceList().get(0));
        Log.d("test0", "réponse 2 sauvegardée : " + q.getChoiceList().get(1));
        Log.d("test0", "réponse 3 sauvegardée : " + q.getChoiceList().get(2));
        Log.d("test0", "réponse 4 sauvegardée : " + q.getChoiceList().get(3));
        Log.d("test0", "Index sauvegardé : " + q.getAnswerIndex());
        Log.d("test0", "--------------------------");



        //prise en charge - écriture UTF8 - test
        FileOutputStream fos = new FileOutputStream(MY_FILE_NAME);
        OutputStreamWriter osw = new OutputStreamWriter(fos, StandardCharsets.UTF_8);
        BufferedWriter writer = new BufferedWriter(osw);
        writer.append(q.getQuestion());
        for(int i = 0; i < 4; i++){
            writer.append(q.getChoiceList().get(i));
        }
        writer.append(Integer.toString(q.getAnswerIndex()));
        /*FileOutputStream outputStream = openFileOutput(MY_FILE_NAME, MODE_APPEND);
        outputStream.write(q.getQuestion().getBytes());
        outputStream.write('\n');
        for(int i = 0; i < 4; i++){
            outputStream.write(q.getChoiceList().get(i).getBytes());
            outputStream.write('\n');
        }
        outputStream.write(Integer.toString(q.getAnswerIndex()).getBytes());
        outputStream.write('\n');
        outputStream.close()*/
        fos.close();

        //prise en charge - lecture UTF8 - test
        Log.d("test-UTF8", "1");
        FileInputStream fis = new FileInputStream(MY_FILE_NAME);
        Log.d("test-UTF8", "2");
        InputStreamReader isr = new InputStreamReader(fis, StandardCharsets.UTF_8);
        Log.d("test-UTF8", "3");
        BufferedReader reader = new BufferedReader(isr);
        Log.d("test-UTF8", "4");
        String str;
        if(reader.readLine() == null){
            Log.d("test-UTF8", "readLine vide");
        }
        Log.d("test-UTF8", "5");
        while ((str = reader.readLine()) != null) {
            Log.d("test-UTF8", str);
        }
        fis.close();
    }

    private QuestionBank loadQuestions() throws IOException {
        String ligne = null;
        int content;
        int i = 0;
        Question q;
        String question = "";
        int answerIndex = -1;
        List<String> answers = new ArrayList<String>();
        List<Question> listQuestions = new ArrayList<Question>();
        QuestionBank qb = null;

        FileInputStream inputStream = openFileInput(MY_FILE_NAME);
        StringBuilder stringb = new StringBuilder();

        while ((content = inputStream.read()) != -1) {
            if(content != '\n'){
                ligne = String.valueOf(stringb.append((char) content));
            } else if (i == 0){
                question = ligne;
                Log.d("test_load", "question in : " + question);
                i++;
                stringb.setLength(0);
            } else if(i == 5){
                answerIndex = Integer.parseInt(ligne);
                Log.d("test_load", "answerIndex in : " + answerIndex);
                i = 0;
                stringb.setLength(0);
                q = new Question(question, answers, answerIndex);
                answers = new ArrayList<String>();
                listQuestions.add(q);
            } else {
                answers.add(ligne);
                Log.d("test_load", "reponse in " + i + " : " + ligne);
                i++;
                stringb.setLength(0);
            }
        }
        qb = new QuestionBank(listQuestions);

        for(i = 0; i < listQuestions.size(); i++){
            Log.d("test", "--------------------------");
            Log.d("test", "Question chargée : " + listQuestions.get(i).getQuestion());
            Log.d("test", "réponse 1 chargée : " + listQuestions.get(i).getChoiceList().get(0));
            Log.d("test", "réponse 2 chargée : " + listQuestions.get(i).getChoiceList().get(1));
            Log.d("test", "réponse 3 chargée : " + listQuestions.get(i).getChoiceList().get(2));
            Log.d("test", "réponse 4 chargée : " + listQuestions.get(i).getChoiceList().get(3));
            Log.d("test", "Index chargé : " + listQuestions.get(i).getAnswerIndex());
        }
        Log.d("test", "--------------------------");
        Log.d("test", "taille de qb : " + qb.getTaille());
        Log.d("test", "Question de 1 qb : " + qb.getQuestion().getQuestion());
        Log.d("test", "Question de 2 qb : " + qb.getQuestion().getQuestion());
        Log.d("test", "Question de 3 qb : " + qb.getQuestion().getQuestion());
        Log.d("test", "Question de 4 qb : " + qb.getQuestion().getQuestion());
        Log.d("test", "Question de 5 qb : " + qb.getQuestion().getQuestion());
        inputStream.close();
        return qb;
    }

    /**
     * Créer la banque de question
     *
     * @return une liste de question
     */
    private QuestionBank generateQuestions() throws IOException {

        Question question1 = new Question((getString(R.string.question1)),
            Arrays.asList(getString(R.string.response11), getString(R.string.response12), getString(R.string.response13), getString(R.string.response14)),
            2);

    Question question2 = new Question((getString(R.string.question2)),
            Arrays.asList(getString(R.string.response21), getString(R.string.response22), getString(R.string.response23), getString(R.string.response24)),
            2);

    Question question3 = new Question((getString(R.string.question3)),
            Arrays.asList(getString(R.string.response31), getString(R.string.response32), getString(R.string.response33), getString(R.string.response34)),
            1);

    Question question4 = new Question((getString(R.string.question4)),
            Arrays.asList(getString(R.string.response41), getString(R.string.response42), getString(R.string.response43), getString(R.string.response44)),
            0);

    Question question5 = new Question((getString(R.string.question5)),
            Arrays.asList(getString(R.string.response51), getString(R.string.response52), getString(R.string.response53), getString(R.string.response54)),
            0);

        return new QuestionBank(Arrays.asList(question1,
                                question2,
                                question3,
                                question4,
                                question5));
        //return loadQuestions();
}

    @RequiresApi(api = Build.VERSION_CODES.M)
    @Override
    public void onClick(View v) {
        int responseIndex = (int) v.getTag();

        if (responseIndex == mCurrentQuestion.getAnswerIndex()) {
            mUser.incScoreByOne();
            gestionCouleurBouton(responseIndex, true);
            Toast.makeText(this, getString(R.string.goodAnswer), Toast.LENGTH_SHORT).show();
        } else {
            gestionCouleurBouton(responseIndex, false);
            Toast.makeText(this, getString(R.string.badAnswer), Toast.LENGTH_SHORT).show();
        }

        mEnableTouchEvents = false;

        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {
                mEnableTouchEvents = true;
                if (--mNumberOfQuestions == 0) {
                    endGame();
                } else {
                    mCurrentQuestion = mQuestionBank.getQuestion();
                    //Log.d("test", "Question courante : " + mCurrentQuestion.getQuestion());
                    gestionCouleurBouton(4, true);
                    displayQuestion(mCurrentQuestion);
                }
            }
        }, 2000);
    }

    /**
     * Permet de colorer l'affichage des boutons après réponse à une question
     *
     * @param numQuestion : est le numéro de la question, 4 pour réinitialiser les boutons avec les
     *                    couleurs d'origine
     * @param reponse     : si la réponse est vrai ou fausse
     */
    @RequiresApi(api = Build.VERSION_CODES.M)
    void gestionCouleurBouton(int numQuestion, boolean reponse) {
        switch (numQuestion) {
            case 0:
                if (reponse) {
                    mAnswerButton1.setBackgroundColor(getColor(R.color.colorGreen));
                } else {
                    mAnswerButton1.setBackgroundColor(getColor(R.color.colorRed));
                    mAnswerButton1.setTextColor(getColor(R.color.colorPrimaryDark));
                }
                break;
            case 1:
                if (reponse) {
                    mAnswerButton2.setBackgroundColor(getColor(R.color.colorGreen));
                } else {
                    mAnswerButton2.setBackgroundColor(getColor(R.color.colorRed));
                    mAnswerButton2.setTextColor(getColor(R.color.colorPrimaryDark));
                }
                break;
            case 2:
                if (reponse) {
                    mAnswerButton3.setBackgroundColor(getColor(R.color.colorGreen));
                } else {
                    mAnswerButton3.setBackgroundColor(getColor(R.color.colorRed));
                    mAnswerButton3.setTextColor(getColor(R.color.colorPrimaryDark));
                }
                break;
            case 3:
                if (reponse) {
                    mAnswerButton4.setBackgroundColor(getColor(R.color.colorGreen));
                } else {
                    mAnswerButton4.setBackgroundColor(getColor(R.color.colorRed));
                    mAnswerButton4.setTextColor(getColor(R.color.colorPrimaryDark));
                }
                break;
            case 4:
                mAnswerButton1.setBackgroundColor(getColor(R.color.colorWhite));
                mAnswerButton2.setBackgroundColor(getColor(R.color.colorWhite));
                mAnswerButton3.setBackgroundColor(getColor(R.color.colorWhite));
                mAnswerButton4.setBackgroundColor(getColor(R.color.colorWhite));
                mAnswerButton1.setTextColor(getColor(R.color.colorAccent));
                mAnswerButton2.setTextColor(getColor(R.color.colorAccent));
                mAnswerButton3.setTextColor(getColor(R.color.colorAccent));
                mAnswerButton4.setTextColor(getColor(R.color.colorAccent));
                break;
        }
    }

    @Override
    public boolean dispatchTouchEvent(MotionEvent ev) {
        return mEnableTouchEvents && super.dispatchTouchEvent(ev);
    }

    private void endGame() {

        AlertDialog.Builder builder = new AlertDialog.Builder(this);

        builder.setTitle(getString((R.string.congrats)))
                .setMessage(getString(R.string.messageScore) + mUser.getScore() + getString(R.string.points))
                .setPositiveButton(getString(R.string.OKbutton), new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        finish();
                    }
                })
                .create()
                .show();
    }

    @Override
    public void finish() {
            Intent intent = new Intent();
            intent.putExtra(USER, mUser);
            setResult(RESULT_OK, intent);
            super.finish();
    }

    @Override
    protected void onSaveInstanceState(Bundle outState) {
        outState.putInt(BUNDLE_STATE_SCORE, mUser.getScore());
        outState.putInt(BUNDLE_STATE_QUESTION, mNumberOfQuestions);
        outState.putParcelable(BUNDLE_QUESTION_BANK, mQuestionBank);
        super.onSaveInstanceState(outState);
    }

    @Override
    protected void onStart() {
        super.onStart();
        System.out.println("GameActivity::onStart()");
    }

    @Override
    protected void onResume() {
        super.onResume();
        System.out.println("GameActivity::onResume()");
    }

    @Override
    protected void onPause() {
        super.onPause();
        System.out.println("GameActivity::onPause()");
    }

    @Override
    protected void onStop() {
        super.onStop();
        System.out.println("GameActivity::onStop()");
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        System.out.println("GameActivity::onDestroy()");
    }
}
