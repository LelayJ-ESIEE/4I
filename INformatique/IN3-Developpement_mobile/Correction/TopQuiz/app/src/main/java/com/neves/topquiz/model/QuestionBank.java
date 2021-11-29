package com.neves.topquiz.model;

import android.os.Parcel;
import android.os.Parcelable;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class QuestionBank implements Parcelable{
    private List<Question> mQuestionList;
    private int mNextQuestionIndex;

    public int getTaille() {
        return taille;
    }

    public void setTaille(int taille) {
        this.taille = taille;
    }

    private int taille;

    /**
     * Constructeur
     * @param questionList : Liste de questions
     */
    public QuestionBank(List<Question> questionList) {
        mQuestionList = questionList;
        Collections.shuffle(mQuestionList);
        mNextQuestionIndex = 0;
        taille = questionList.size();
    }

    /**
     * Permet l'ajout d'une question à la banque
     * @param question
     */
    public void setQuestion(Question question){
        mQuestionList.add(question);
        mNextQuestionIndex++;
    }

    /**
     * Renvoie la question actuelle de la liste
     * @return : une question
     */
    public Question getCurrentQuestion() {
        return mQuestionList.get(mNextQuestionIndex);
    }

    /**
     * Modifie l'index de la question en court dans la liste
     * @param mNextQuestionIndex
     */
    public void setmNextQuestionIndex(int mNextQuestionIndex){
        this.mNextQuestionIndex = mNextQuestionIndex;
    }

    /**
     * Renvoie la question suivante de la liste
     * @return : une question
     */
    public Question getQuestion() {
        if(mNextQuestionIndex == mQuestionList.size()){
            mNextQuestionIndex = 0;
        }
        return mQuestionList.get(mNextQuestionIndex++);
    }

    public int describeContents() {
        return 0;
    }

    public void writeToParcel(Parcel out, int flags) {
        out.writeList(mQuestionList);
        out.writeInt(mNextQuestionIndex);
    }

    public static final Parcelable.Creator<QuestionBank> CREATOR
            = new Parcelable.Creator<QuestionBank>() {
        public QuestionBank createFromParcel(Parcel in) {
            return new QuestionBank(in);
        }

        public QuestionBank[] newArray(int size) {
            return new QuestionBank[size];
        }
    };

    private QuestionBank(Parcel in) {
        mQuestionList = new ArrayList<Question>();
        in.readList(mQuestionList, Question.class.getClassLoader());
        mNextQuestionIndex = in.readInt();
    }
}
