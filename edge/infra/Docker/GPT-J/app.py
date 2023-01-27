# Reference: https://huggingface.co/docs/transformers/model_doc/gptj
from flask import Flask, request, jsonify
from transformers import GPTJForCausalLM, AutoTokenizer
import torch

app = Flask(__name__)

# Initialize the GPT-J model
model = GPTJForCausalLM.from_pretrained("EleutherAI/gpt-j-6B", torch_dtype=torch.float16)
tokenizer = AutoTokenizer.from_pretrained("EleutherAI/gpt-j-6B")

@app.route('/generate', methods=['POST'])
def generate():
    # Get the input text from the request body
    input_text = request.json['input_text']

    # Sample prompt (input_text)
    #prompt = (
    #    "In a shocking finding, scientists discovered a herd of unicorns living in a remote, "
    #    "previously unexplored valley, in the Andes Mountains. Even more surprising to the "
    #    "researchers was the fact that the unicorns spoke perfect English."
    #)

    # Generate the output text using the GPT-J model
    input_ids = tokenizer(input_text, return_tensors="pt").input_ids

    gen_tokens = model.generate(
        input_ids,
        do_sample=True,
        temperature=0.9,
        max_length=100,
    )

    gen_text = tokenizer.batch_decode(gen_tokens)[0]
    
    # Return the output text as JSON
    return jsonify({'output_text': output_text})

if __name__ == '__main__':
    app.run()