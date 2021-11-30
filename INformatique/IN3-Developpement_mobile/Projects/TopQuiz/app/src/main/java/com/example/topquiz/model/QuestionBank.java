package com.example.topquiz.model;

import android.os.Parcel;
import android.os.Parcelable;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class QuestionBank implements Parcelable {
    private List<Question> mQuestionList;
    private int mQuestionIndex;

    public QuestionBank(List<Question> questionList) {
        mQuestionList = questionList;
        Collections.shuffle(mQuestionList);
        mQuestionIndex = 0;
    }

    protected QuestionBank(Parcel in) {
        mQuestionList = new ArrayList<Question>();
        in.readList(mQuestionList, Question.class.getClassLoader());
        mQuestionIndex = in.readInt();
    }

    public static final Creator<QuestionBank> CREATOR = new Creator<QuestionBank>() {
        @Override
        public QuestionBank createFromParcel(Parcel in) {
            return new QuestionBank(in);
        }

        @Override
        public QuestionBank[] newArray(int size) {
            return new QuestionBank[size];
        }
    };

    public Question getCurrentQuestion() {
        return mQuestionList.get(mQuestionIndex);
    }

    public Question getNextQuestion() {
        mQuestionIndex++;
        return getCurrentQuestion();
    }

    public void setQuestionIndex(int questionIndex) {
        mQuestionIndex = questionIndex;
    }

    @Override
    public int describeContents() {
        return 0;
    }

    @Override
    public void writeToParcel(Parcel dest, int flags) {
        dest.writeList(mQuestionList);
        dest.writeInt(mQuestionIndex);
    }
}
