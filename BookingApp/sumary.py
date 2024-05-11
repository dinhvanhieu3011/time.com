
import os
import datetime
from concurrent.futures import ThreadPoolExecutor
from tqdm import tqdm
from litellm import completion

def read_string_array1(filename):
  data = []
  with open(filename, "r") as f:
    for line in f:
      data.append(line.strip())  # Remove trailing newline character
  return data


def summarize(text_array):
    """
    Summarize the text using GPT API
    """

    def create_chunks(paragraphs):
        chunks = []
        chunk = ''
        for paragraph in paragraphs:
            if len(chunk) + len(paragraph) < 10000:
                chunk += paragraph + ' '
            else:
                chunks.append(chunk.strip())
                chunk = paragraph + ' '
        if chunk:
            chunks.append(chunk.strip())
        return chunks

    try:
        text_chunks = create_chunks(text_array)
        text_chunks = [chunk for chunk in text_chunks if chunk] # Remove empty chunks

        # Call the GPT API in parallel to summarize the text chunks
        summaries = []
        system_messages = [
            {"role": "system", "content": "You are an expert in creating summaries that capture the main points and key details."},
            {"role": "system", "content": f"You will show the bulleted list content without translate any technical terms."},
            {"role": "system", "content": f"You will print all the content in English."},
        ]
        with ThreadPoolExecutor() as executor:
            futures = [executor.submit(call_gpt_api, f"Summary keypoints for the following text:\n{chunk}", system_messages) for chunk in text_chunks]
            for future in tqdm(futures, total=len(text_chunks), desc="Summarizing"):
                summaries.append(future.result())

        if len(summaries) <= 5:
            summary = ' '.join(summaries)
            with tqdm(total=1, desc="Final summarization") as progress_bar:
                final_summary = call_gpt_api(f"Create a bulleted list using English to show the key points of the following text:\n{summary}", system_messages)
                progress_bar.update(1)
            return final_summary
        else:
            return summarize(summaries)
    except Exception as e:
        print(f"Error: {e}")
        return "Unknown error! Please contact the developer."
    
def call_gpt_api(prompt, additional_messages=[]):
    """
    Call GPT API
    """
    try:
        response = completion(
        # response = openai.ChatCompletion.create(
            model='gpt-4-turbo',
            messages=additional_messages+[
                {"role": "user", "content": prompt}
            ],
            api_key = "sk-proj-ib0ggYPlkskFdaPixmQQT3BlbkFJdVwH6Dr0xWg3EawJ2SKP"

        )
        message = response.choices[0].message.content.strip()
        return message
    except Exception as e:
        print(f"Error: {e}")
        return ""


def main():
    try:
       x = datetime.datetime.now()
       y = datetime.datetime(2024, 5, 10)
       if(x < y )  :         
            print('Nhập đường file txt path: (ví dụ: c:\\Users\\hieu\\tool-youtube\\temp.txt.eng.txt)')
            x = input()
            paragraph = read_string_array1(x)
            #paragraph = read_string_array1('c:\\Users\\hieu\\tool-youtube\\temp.txt.eng.txt')
            result = summarize(paragraph)
            print(result)
       else:
            print('API key hết hạn')
           
    except Exception as e:
        print(e)

if __name__ == '__main__':
    main()