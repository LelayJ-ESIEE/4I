package com.neves.topquiz.model;

import android.os.Parcel;
import android.os.Parcelable;

import java.util.ArrayList;
import java.util.List;

public class Question /*implements Parcelable*/{
    private String mQuestion;
    private List<String> mChoiceList;
    private int mAnswerIndex;

    /**
     * Constructeur
     *
     * @param question    : Une question
     * @param choiceList  : La liste des quatre réponses
     * @param answerIndex : Le numéro de la réponse à la question
     */
    public Question(String question, List<String> choiceList, int answerIndex) {
        if ((answerIndex >= 0 && answerIndex < 4) && (choiceList.size() != 0) && question.length() > 0) {
            mQuestion = question;
            mChoiceList = choiceList;
            mAnswerIndex = answerIndex;
        }
    }

    /**
     * Retourner une question
     *
     * @return Une question
     */
    public String getQuestion() {
        return mQuestion;
    }

    /**
     * Retourne la liste des réponses
     *
     * @return la liste des réponses
     */
    public List<String> getChoiceList() {
        return mChoiceList;
    }

    /**
     * Retourne le numéro de la réponse à la question
     *
     * @return le numéro de la réponse à la question
     */
    public int getAnswerIndex() {
        return mAnswerIndex;
    }

    /*public int describeContents() {
        return 0;
    }

    public void writeToParcel(Parcel out, int flags) {
        out.writeString(mQuestion);
        out.writeList(mChoiceList);
        out.writeInt(mAnswerIndex);
    }

    public static final Parcelable.Creator<Question> CREATOR
            = new Parcelable.Creator<Question>() {
        public Question createFromParcel(Parcel in) {
            return new Question(in);
        }

        public Question[] newArray(int size) {
            return new Question[size];
        }
    };

    private Question(Parcel in) {
        mQuestion = in.readString();
        mChoiceList = new ArrayList<String>();
        in.readList(mChoiceList, Question.class.getClassLoader());
        mAnswerIndex = in.readInt();
    }*/
}
