package com.example.topquiz.controller;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.example.topquiz.R;
import com.example.topquiz.model.Question;
import com.example.topquiz.model.QuestionBank;

import java.util.Arrays;

public class GameActivity extends AppCompatActivity implements View.OnClickListener {
    public static final String TAG = "GameActivity";
    public static final String BUNDLE_EXTRA_SCORE = "BUNDLE_EXTRA_SCORE";
    public static final String BUNDLE_STATE_QUESTION = "BUNDLE_STATE_QUESTION";
    public static final String BUNDLE_STATE_SCORE = "BUNDLE_STATE_SCORE";
    private TextView mQuestionTextView;
    private Button mAnswerButton1;
    private Button mAnswerButton2;
    private Button mAnswerButton3;
    private Button mAnswerButton4;
    private QuestionBank mQuestionBank;
    private boolean mEnableTouchEvents;
    private int mRemainingQuestionCount = 3;
    private int mScore = 0;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        // Log.d(TAG, "onCreate() called");

        if (savedInstanceState != null) {
            mScore = savedInstanceState.getInt(BUNDLE_STATE_SCORE);
            mRemainingQuestionCount = savedInstanceState.getInt(BUNDLE_STATE_QUESTION);
        } else {
            mScore = 0;
            mRemainingQuestionCount = 3;
        }

        setContentView(R.layout.activity_game);

        mQuestionBank = generateQuestionBank();
        mEnableTouchEvents = true;

        mQuestionTextView = (TextView) findViewById(R.id.game_activity_textview_question);
        mAnswerButton1 = (Button) findViewById(R.id.game_activity_button_1);
        mAnswerButton2 = (Button) findViewById(R.id.game_activity_button_2);
        mAnswerButton3 = (Button) findViewById(R.id.game_activity_button_3);
        mAnswerButton4 = (Button) findViewById(R.id.game_activity_button_4);

        mAnswerButton1.setEnabled(true);
        mAnswerButton2.setEnabled(true);
        mAnswerButton3.setEnabled(true);
        mAnswerButton4.setEnabled(true);

        mAnswerButton1.setOnClickListener(this);
        mAnswerButton2.setOnClickListener(this);
        mAnswerButton3.setOnClickListener(this);
        mAnswerButton4.setOnClickListener(this);

        this.displayQuestion(mQuestionBank.getCurrentQuestion());
    }

    private QuestionBank generateQuestionBank(){
        Question question1 = new Question(
                getString(R.string.question1),
                Arrays.asList(
                        getString(R.string.answer11),
                        getString(R.string.answer12),
                        getString(R.string.answer13),
                        getString(R.string.answer14)
                ),
                0
        );

        Question question2 = new Question(
                getString(R.string.question2),
                Arrays.asList(
                        getString(R.string.answer21),
                        getString(R.string.answer22),
                        getString(R.string.answer23),
                        getString(R.string.answer24)
                ),
                3
        );

        Question question3 = new Question(
                getString(R.string.question3),
                Arrays.asList(
                        getString(R.string.answer31),
                        getString(R.string.answer32),
                        getString(R.string.answer33),
                        getString(R.string.answer34)
                ),
                3
        );

        Question question4 = new Question(
                getString(R.string.question4),
                Arrays.asList(
                        getString(R.string.answer41),
                        getString(R.string.answer42),
                        getString(R.string.answer43),
                        getString(R.string.answer44)
                ),
                2
        );

        Question question5 = new Question(
                getString(R.string.question5),
                Arrays.asList(
                        getString(R.string.answer51),
                        getString(R.string.answer52),
                        getString(R.string.answer53),
                        getString(R.string.answer54)
                ),
                1
        );

        Question question6 = new Question(
                getString(R.string.question6),
                Arrays.asList(
                        getString(R.string.answer61),
                        getString(R.string.answer62),
                        getString(R.string.answer63),
                        getString(R.string.answer64)
                ),
                0
        );

        return new QuestionBank(Arrays.asList(question1, question2, question3, question4, question5, question6));
    }

    private void displayQuestion(final Question question) {
        mQuestionTextView.setText(question.getQuestion());
        mAnswerButton1.setText(question.getChoiceList().get(0));
        mAnswerButton2.setText(question.getChoiceList().get(1));
        mAnswerButton3.setText(question.getChoiceList().get(2));
        mAnswerButton4.setText(question.getChoiceList().get(3));
    }

    private void endGame(){
        AlertDialog.Builder builder = new AlertDialog.Builder(this);

        builder.setTitle("Well done!")
                .setMessage("Your score is " + mScore)
                .setPositiveButton("OK", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        Intent intent = new Intent();
                        intent.putExtra(BUNDLE_EXTRA_SCORE, mScore);
                        setResult(RESULT_OK, intent);
                        finish();
                    }
                })
                .create()
                .show();
    }

    @Override
    public boolean dispatchTouchEvent(MotionEvent ev) {
        return mEnableTouchEvents && super.dispatchTouchEvent(ev);
    }

    @Override
    public void onClick(View v) {
        int index;

        if (v == mAnswerButton1) {
            index = 0;
        } else if (v == mAnswerButton2) {
            index = 1;
        } else if (v == mAnswerButton3) {
            index = 2;
        } else if (v == mAnswerButton4) {
            index = 3;
        } else {
            throw new IllegalStateException("Unknown clicked view : " + v);
        }

        if(index == mQuestionBank.getCurrentQuestion().getAnswerIndex()) {
            mScore++;
            Toast.makeText(this, "Correct!", Toast.LENGTH_SHORT).show();
        }
        else
            Toast.makeText(this, "Wrong!", Toast.LENGTH_SHORT).show();

        this.mRemainingQuestionCount--;

        mEnableTouchEvents = false;

        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {
                mEnableTouchEvents = true;
                if (mRemainingQuestionCount > 0)
                    displayQuestion(mQuestionBank.getNextQuestion());
                else
                    endGame();
            }
        }, 2000);
    }

    @Override
    protected void onStart() {
        super.onStart();
        // Log.d(TAG, "onStart() called");
    }

    @Override
    protected void onResume() {
        super.onResume();
        // Log.d(TAG, "onResume() called");
    }

    @Override
    protected void onPause() {
        super.onPause();
        // Log.d(TAG, "onPause() called");
    }

    @Override
    protected void onStop() {
        super.onStop();
        // Log.d(TAG, "onStop() called");
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        // Log.d(TAG, "onDestroy() called");
    }

    @Override
    protected void onSaveInstanceState(@NonNull Bundle outState) {
        super.onSaveInstanceState(outState);

        outState.putInt(BUNDLE_STATE_SCORE, mScore);
        outState.putInt(BUNDLE_STATE_QUESTION, mRemainingQuestionCount);
    }

}